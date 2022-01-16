using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces.Metrics
{
    public interface ICpuMetricsRepository: IMetricsRepository<CpuMetric>
    {
    }
}
