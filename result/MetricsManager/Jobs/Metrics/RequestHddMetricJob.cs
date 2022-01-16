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
    public class RequestHddMetricJob : IJob
    {
        private IHddMetricsRepository hddRepository;
        private IAgentsInfoRepository agentsRepository;
        private IMetricsAgentClient client;
        public RequestHddMetricJob(IHddMetricsRepository hddRepository, IAgentsInfoRepository agentsRepository, IMetricsAgentClient client)
        {
            this.hddRepository = hddRepository;
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
            RequestHddMetricToAgent request = await CreateRequest(agent);
            ResponseHddMetricFromAgent response = await client.GetHddMetric(request);
            if (response == null)
            {
                return;
            }
            foreach (var metric in response.Metrics)
            {
                await hddRepository.Create(new HddMetric
                {
                    AgentId = agent.AgentId,
                    MetricId = metric.Id,
                    Value = metric.Value,
                    Time = TimeSpan.FromSeconds(metric.Time)
                });
            }
        }
        public async Task<RequestHddMetricToAgent> CreateRequest(AgentInfo agent)
        {
            HddMetric lastValue = await hddRepository.GetLastValue(agent.AgentId);
            TimeSpan time = TimeSpan.FromSeconds(0);
            if (lastValue != null)
            {
                time = TimeSpan.FromSeconds(lastValue.Time.TotalSeconds + 1);
            }

            return new RequestHddMetricToAgent
            {
                Agent = agent,
                fromTime = time,
                toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };
        }
    }
}
