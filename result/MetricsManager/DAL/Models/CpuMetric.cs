using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models
{
    public class CpuMetric
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public double Time { get; set; }
    }
}
