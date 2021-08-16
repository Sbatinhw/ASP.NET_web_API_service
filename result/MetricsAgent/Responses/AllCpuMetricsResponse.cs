using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    [Serializable]
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }

    
}