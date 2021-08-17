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
        int agentId = 3;
        string connectionManager = "http://localhost:5000";

        public List<double> GetMetric(CpuRequest request)
        {

            string requestAdress = $"{connectionManager}/api/cpumetrics/agent/{agentId}/from/{request.FromTime}/to/{request.ToTime}";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestAdress);
            var httpClient = new HttpClient();

            List<double> result = new List<double>();

            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                var res = JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result;

                foreach(var metric in res.Metrics)
                {
                    result.Add(metric.Value);
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("не успешно");

            }
            return result;



        }

    }
}
