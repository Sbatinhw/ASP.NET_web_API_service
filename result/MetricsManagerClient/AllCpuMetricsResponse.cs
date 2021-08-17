using System;
using System.Collections.Generic;

namespace MetricsManagerClient
{
    [Serializable]
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }

    
}