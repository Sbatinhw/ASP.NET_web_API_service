using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Request.ResponseFromAgent.Metrics
{
    public class CpuMetricFromAgent
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int Time { get; set; }
    }
}
