using AutoMapper;
using MetricsManager.DAL.Interfaces.Metrics;
using MetricsManager.DAL.Models;
using MetricsManager.Response;
using MetricsManager.Response.DTO.AgentMetric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository repository;
        private readonly IMapper mapper;
        private ILogger logger;

        public HddMetricsController(IHddMetricsRepository repository, IMapper mapper, ILogger<HddMetricsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("cluster/agentid/{agentId}/from/{fromTime}/to/{toTime}")]
        public async Task<IActionResult> GetMetricsByTimePeriod(
            [FromRoute] int agentId,
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            logger.LogInformation($"Запрос метрик ПЗУ агента {agentId} с {fromTime} по {toTime}");

            GetHddMetricsResponse result = new GetHddMetricsResponse
            {
                Metrics = new List<HddMetricDto>()
            };

            List<HddMetric> metrics = await repository.GetByTimePeriod(agentId, fromTime, toTime);

            foreach (var metric in metrics)
            {
                result.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }

            logger.LogInformation($"Отдано метрик ПЗУ агента {agentId} {result.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(result));
        }
    }
}
