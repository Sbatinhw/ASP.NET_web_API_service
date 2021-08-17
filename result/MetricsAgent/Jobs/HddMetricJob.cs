using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Models;
using Quartz;
using MetricsAgent.DAL;
using System.Diagnostics;
using System.IO;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricRepository _repository;
        private long _hddCounter;
        private string path;

        public HddMetricJob(IHddMetricRepository repository)
        {
            _repository = repository;

            //узнаём в какой директории выполняется программа
            path = Directory.GetCurrentDirectory();
            //узнаём на каком диске находится директория
            path = Directory.GetDirectoryRoot(path);

            //узнаём свободное место
            _hddCounter = new DriveInfo(path).TotalFreeSpace;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var hddAvailableBytes = _hddCounter;
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds;
            _repository.Create(new HddMetric { Time = time, Value = _hddCounter });

            return Task.CompletedTask;
        }


    }
}
