using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    [Serializable]
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }

    
}