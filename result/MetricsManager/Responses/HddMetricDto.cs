using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
    public class HddMetricDto
    {
        public double Time { get; set; }
        public long Value { get; set; }
        public int Id { get; set; }
    }
}
