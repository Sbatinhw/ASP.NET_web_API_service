using MetricsAgent.Responses.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class GetHddMetricsResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }
}
