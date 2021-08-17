using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob :IJob
    {
        private IRamMetricRepository _repository;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramAvailableMBytes = Convert.ToInt32(_ramCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds;
            _repository.Create(new RamMetric { Time = time, Value = ramAvailableMBytes });

            return Task.CompletedTask;
        }

    }
}
