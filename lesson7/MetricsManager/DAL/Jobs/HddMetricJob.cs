using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Models;
using Quartz;
using MetricsManager.DAL;
using System.Diagnostics;
using System.IO;

namespace MetricsManager.Jobs
{
    public class HddMetricJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
