using MetricsManager.Request.RequestToAgent;
using MetricsManager.Request.ResponseFromAgent.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        Task<ResponseCpuMetricFromAgent> GetCpuMetric(RequestCpuMetricToAgent request);
        Task<ResponseRamMetricFromAgent> GetRamMetric(RequestRamMetricToAgent request);
        Task<ResponseHddMetricFromAgent> GetHddMetric(RequestHddMetricToAgent request);
    }
}
