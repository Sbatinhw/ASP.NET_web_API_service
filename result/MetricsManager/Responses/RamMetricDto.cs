using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
    public class RamMetricDto
    {
        public double Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
