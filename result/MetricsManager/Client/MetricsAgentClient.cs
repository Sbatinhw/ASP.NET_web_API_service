using MetricsManager.Request.RequestToAgent;
using MetricsManager.Request.ResponseFromAgent.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }
        public async Task<ResponseCpuMetricFromAgent> GetCpuMetric(RequestCpuMetricToAgent request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.ConnectionLine);
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ResponseCpuMetricFromAgent>(responseStream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<ResponseHddMetricFromAgent> GetHddMetric(RequestHddMetricToAgent request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.ConnectionLine);
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ResponseHddMetricFromAgent>(responseStream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<ResponseRamMetricFromAgent> GetRamMetric(RequestRamMetricToAgent request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.ConnectionLine);
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ResponseRamMetricFromAgent>(responseStream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
