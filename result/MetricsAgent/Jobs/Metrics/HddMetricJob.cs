using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs.Metrics
{
    public class HddMetricJob: IJob
    {
        private IHddMetricsRepository repository;

        public HddMetricJob(IHddMetricsRepository repository)
        {
            this.repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //узнаём в какой директории выполняется программа
            string path = Directory.GetCurrentDirectory();
            //узнаём на каком диске находится директория
            path = Directory.GetDirectoryRoot(path);

            //узнаём свободное место
            long hddCounter = new DriveInfo(path).TotalFreeSpace;

            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            repository.Create(new HddMetric { Time = time, Value = hddCounter });

            return Task.CompletedTask;
        }
    }
}
