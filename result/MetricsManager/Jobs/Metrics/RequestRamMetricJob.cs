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
    public class RequestRamMetricJob : IJob
    {
        private IRamMetricsRepository ramRepository;
        private IAgentsInfoRepository agentsRepository;
        private IMetricsAgentClient client;
        public RequestRamMetricJob(IRamMetricsRepository ramRepository, IAgentsInfoRepository agentsRepository, IMetricsAgentClient client)
        {
            this.ramRepository = ramRepository;
            this.agentsRepository = agentsRepository;
            this.client = client;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            IList<AgentInfo> agents = await agentsRepository.GetAll();
            foreach (var agent in agents)
            {
                await Work(agent);
            }

        }
        public async Task Work(AgentInfo agent)
        {
            RequestRamMetricToAgent request = await CreateRequest(agent);
            ResponseRamMetricFromAgent response = await client.GetRamMetric(request);
            if (response == null)
            {
                return;
            }
            foreach (var metric in response.Metrics)
            {
                await ramRepository.Create(new RamMetric
                {
                    AgentId = agent.AgentId,
                    MetricId = metric.Id,
                    Value = metric.Value,
                    Time = TimeSpan.FromSeconds(metric.Time)
                });
            }
        }
        public async Task<RequestRamMetricToAgent> CreateRequest(AgentInfo agent)
        {
            RamMetric lastValue = await ramRepository.GetLastValue(agent.AgentId);
            TimeSpan time = TimeSpan.FromSeconds(0);
            if (lastValue != null)
            {
                time = TimeSpan.FromSeconds(lastValue.Time.TotalSeconds + 1);
            }

            return new RequestRamMetricToAgent
            {
                Agent = agent,
                fromTime = time,
                toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };
        }
    }
}
