using MetricsManager.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using MetricsManager.Models;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using System.Collections.Generic;
using MetricsManager.Client;
using AutoMapper;
using MetricsManager.Responses;


namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository repository;

        private const string LocalConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        private IMetricsAgentClient metricsAgentClient;
        private readonly IMapper mapper;



        public CpuMetricJob(
            ICpuMetricsRepository repository, 
            IMetricsAgentClient metricsAgentClient, 
            IMapper mapper)
        {
            this.repository = repository;
            this.metricsAgentClient = metricsAgentClient;
            this.mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            ///список агентов
            List<AgentInfo> listAgents = new List<AgentInfo>();

            using (var connection = new SQLiteConnection(LocalConnectionString))
            {
                listAgents = connection.Query<AgentInfo>("SELECT agentid, agenturi FROM agents").ToList();
            }

            ///на случай если агенты записывали данные в разное время
            ///запрашиваем каждого агента по отдельности
            foreach(var agent in listAgents)
            {
                ///время последней полученной метрики
                long timeStart;

                ///получаем время последней метрики
                using (var connection = new SQLiteConnection(LocalConnectionString))
                {
                    timeStart = connection.QuerySingle<long>("SELECT MAX(time) FROM cpumetrics WHERE agentid=@id",
                        new
                        {
                            id = agent.AgentID
                        });
                }

                ///создаём список не сохраненных метрик
                var metricList = metricsAgentClient.GetByIdCpuMetrics(new GetByIdCpuMetricsRequest()
                {
                    FromTime = TimeSpan.FromSeconds(timeStart),
                    ToTime = TimeSpan.FromSeconds(DateTime.Now.Second),
                    Id = agent.AgentID
                }).Metrics;

                ///добавляем метрики в локальную БД
                foreach(var metric in metricList)
                {
                    repository.Create(mapper.Map<CpuMetric>(metric));
                }

            }

            return Task.CompletedTask;
        }
    }
}
