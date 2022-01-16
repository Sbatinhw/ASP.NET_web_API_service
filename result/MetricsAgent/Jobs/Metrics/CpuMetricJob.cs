using MetricsAgent.DAL.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Specialized;
using MetricsAgent.DAL.Models;

namespace MetricsAgent.Jobs.Metrics
{
    public class CpuMetricJob: IJob
    {
        private ICpuMetricsRepository repository;
        private PerformanceCounter cpuCounter;

        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            this.repository = repository;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            repository.Create(new CpuMetric { Time = time, Value = cpuUsageInPercents });

            return Task.CompletedTask;

        }

    }
}
