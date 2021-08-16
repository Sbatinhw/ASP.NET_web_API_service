using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace MetricsManagerClient
{
    class AskCpuMetric
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string connectionString = "http://localhost:5001";
        private int agentId;
        public AskCpuMetric()
        {

        }

        public List<int> GetMetric1(CpuRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{connectionString}/api/CpuMetrics/agent/{agentId}/from/{fromParameter}/to/{toParameter}");
            List<int> result = new List<int>();

            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                var list = JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result.Metrics;
                foreach (var metric in list)
                {
                    result.Add(metric.Value);
                }
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex);
            }
            return result;
        }

        public List<int> GetMetric(CpuRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{connectionString}/api/cpumetrics/agent/{agentId}/from/{fromParameter}/to/{toParameter}");
            List<int> result = new List<int>();
            //Console.WriteLine($"Получение CPU у {request.Id}");

            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                //Console.WriteLine("успешно");

                var list = JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result.Metrics;
                foreach (var metric in list)
                {
                    result.Add(metric.Value);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


    }
}
