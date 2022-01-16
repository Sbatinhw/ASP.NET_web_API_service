using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs.Metrics
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository repository;
        private PerformanceCounter ramCounter;

        public RamMetricJob(IRamMetricsRepository repository)
        {
            this.repository = repository;
            this.ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramAvailableMBytes = Convert.ToInt32(ramCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            repository.Create(new RamMetric { Time = time, Value = ramAvailableMBytes });

            return Task.CompletedTask;
        }

    }
}
