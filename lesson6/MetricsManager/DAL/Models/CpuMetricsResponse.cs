using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models
{
    public class CpuMetricsResponse
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
