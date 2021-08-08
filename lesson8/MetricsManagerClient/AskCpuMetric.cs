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
        private string connectionString = "http://localhost:5000";
        private int agentId = 0;
        public AskCpuMetric()
        {

        }

        public List<int> GetMetric(CpuRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{connectionString}/api/agent/{agentId}/from/{fromParameter}/to/{toParameter}");
            List<int> result = new List<int>();

            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                foreach (var metric in JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result.Metrics)
                {
                    result.Add(metric.Value);
                }
            }
            catch
            {

            }
            return result;
        }

    }
}
