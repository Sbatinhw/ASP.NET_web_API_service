using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.DAL;
using MetricsManager.Responses;
using System.Data.SQLite;
using MetricsManager.Models;
using Dapper;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        public MetricsAgentClient(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public AllCpuMetricsResponse GetByIdCpuMetrics(GetByIdCpuMetricsRequest request)
        {
            string agentUri;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                agentUri = connection.QuerySingle<string>("SELECT agenturi FROM agents WHERE agentid=@id", new
                {
                    id = request.Id
                });
            }

            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentUri}/api/cpumetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;

        }

        public AllCpuMetricsResponse GetCpuMetrics(GetAllCpuMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;

            AllCpuMetricsResponse allMetrics = new AllCpuMetricsResponse() { Metrics = new List<CpuMetricDto>() };
            List<string> agentsUri = new List<string>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                agentsUri = connection.Query<string>("SELECT agenturi FROM agents").ToList();
            }

            foreach(var agentUri in agentsUri)
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentUri}/api/cpumetrics/from/{fromParameter}/to/{toParameter}");
                try
                {
                    HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    allMetrics.Metrics.AddRange( JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result.Metrics);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return allMetrics;

        }

        



    }
}
