using MetricsManager.Request.ResponseFromAgent.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Request.ResponseFromAgent.Responses
{
    public class ResponseCpuMetricFromAgent
    {
        public IList<CpuMetricFromAgent> Metrics { get; set; }
    }
}
