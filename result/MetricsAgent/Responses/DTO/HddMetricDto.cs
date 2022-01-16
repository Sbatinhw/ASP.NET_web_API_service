using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses.DTO
{
    public class HddMetricDto
    {
        public int Id { get; set; }
        public long Value { get; set; }
        public int Time { get; set; }
    }
}
