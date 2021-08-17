using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
    public class GetByIdCpuMetricsRequest
    {
        public int Id { get; set; }
        public double FromTime { get; set; }
        public double ToTime { get; set; }
        public string Uri { get; set; }
    }
}
