using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Models
{
    public class HddMetric
    {
        public int ID { get; set; }
        public long Value { get; set; }
        public double Time { get; set; }
    }
}
