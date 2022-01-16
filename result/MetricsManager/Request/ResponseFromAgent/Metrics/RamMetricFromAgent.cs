using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Request.ResponseFromAgent.Metrics
{
    public class RamMetricFromAgent
    {
        public int Id { get; set; }
        public long Value { get; set; }
        public int Time { get; set; }
    }
}
