using MetricsManager.Client;
using MetricsManager.DAL.Interfaces.Info;
using MetricsManager.DAL.Interfaces.Metrics;
using MetricsManager.DAL.Models;
using MetricsManager.Request.RequestToAgent;
using MetricsManager.Request.ResponseFromAgent.Responses;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs.Metrics
{
    public class RequestCpuMetricJob : IJob
    {
        private ICpuMetricsRepository cpuRepository;
        private IAgentsInfoRepository agentsRepository;
        private IMetricsAgentClient client;
        public RequestCpuMetricJob(ICpuMetricsRepository cpuRepository, IAgentsInfoRepository agentsRepository, IMetricsAgentClient client)
        {
            this.cpuRepository = cpuRepository;
            this.agentsRepository = agentsRepository;
            this.client = client;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            IList<AgentInfo> agents = await agentsRepository.GetAll();
            foreach(var agent  in agents)
            {
                await Work(agent);
            }

        }
        public async Task Work(AgentInfo agent)
        {
            RequestCpuMetricToAgent request = await CreateRequest(agent);
            ResponseCpuMetricFromAgent response = await client.GetCpuMetric(request);
            if(response == null)
            {
                return;
            }
            foreach(var metric in response.Metrics)
            {
                await cpuRepository.Create(new CpuMetric
                {
                    AgentId = agent.AgentId,
                    MetricId = metric.Id,
                    Value = metric.Value,
                    Time = TimeSpan.FromSeconds(metric.Time)
                });
            }
        }
        public async Task<RequestCpuMetricToAgent> CreateRequest(AgentInfo agent)
        {
            CpuMetric lastValue = await cpuRepository.GetLastValue(agent.AgentId);
            TimeSpan time = TimeSpan.FromSeconds(0);
            if (lastValue != null)
            {
                time = TimeSpan.FromSeconds(lastValue.Time.TotalSeconds + 1);
            }

            return new RequestCpuMetricToAgent
            {
                Agent = agent,
                fromTime = time,
                toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };
        }
    }
}
