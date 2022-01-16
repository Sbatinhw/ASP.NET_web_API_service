using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Response.DTO.AgentMetric
{
    public class RamMetricDto
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int MetricId { get; set; }
        public long Value { get; set; }
        public int Time { get; set; }
    }
}
