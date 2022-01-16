using MetricsManager.Response.DTO.AgentMetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Response
{
    public class GetCpuMetricsResponse
    {
        public IList<CpuMetricDto> Metrics { get; set; }
    }
}
