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

            //using (var connection = new SQLiteConnection(LocalConnectionString))
            {
                //connection.Execute("INSERT INTO agents(agentadress) VALUES(@agentadress)",
                    //new
                    //{
                        //agentid = item.ID,
                        //agentadress = "http://localhost:5000".ToString()
                    //});
                //var list = connection.Query<string>("SELECT agentadress FROM agents").ToList();
                //var listAgent = connection.Query<AgentInfo>("SELECT agentid, agenturi FROM agents").ToList();

            }

            ///список агентов
            List<AgentInfo> listAgents = new List<AgentInfo>();

            using (var connection = new SQLiteConnection(LocalConnectionString))
            {
                listAgents = connection.Query<AgentInfo>("SELECT agentid, agentadress FROM agents").ToList();
            }

            ///на случай если агенты записывали данные в разное время
            ///запрашиваем каждого агента по отдельности
            foreach(var agent in listAgents)
            {
                ///время последней полученной метрики
                double timeStart;

                ///получаем время последней метрики
                using (var connection = new SQLiteConnection(LocalConnectionString))
                {
                    try
                    {
                        timeStart = connection.QuerySingle<double>("SELECT MAX(time) FROM cpumetrics WHERE agentid=@id",
                            new
                            {
                                id = agent.AgentID
                            });
                    }
                    catch
                    {
                        timeStart = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(24)).ToUnixTimeSeconds()).TotalSeconds; 
                    }
                }

                List<CpuMetricDto> metricList;

               
                    ///создаём список не сохраненных метрик
                    metricList = metricsAgentClient.GetByIdCpuMetrics(new GetByIdCpuMetricsRequest()
                    {
                        FromTime = TimeSpan.FromSeconds(timeStart),
                        ToTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                        Id = agent.AgentID
                    }).Metrics;

                if (metricList != null)
                {
                    ///добавляем метрики в локальную БД
                    foreach (var metric in metricList)
                    {
                        repository.Create(mapper.Map<CpuMetric>(metric));
                    }
                }


            }

            return Task.CompletedTask;
        }
    }
}
