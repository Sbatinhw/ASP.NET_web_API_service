using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsManager.DAL;
using MetricsManager.Models;
using System.Diagnostics;

namespace MetricsManager.Jobs
{
    public class RamMetricJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
