using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
    public class CpuMetricDto
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }
    }
}
