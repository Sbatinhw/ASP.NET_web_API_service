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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricsRepository repository;
        private readonly IMapper mapper;
        private ILogger logger;

        public RamMetricsController(IRamMetricsRepository repository, IMapper mapper, ILogger<RamMetricsController> logger)
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
            logger.LogInformation($"Запрос метрик ОЗУ агента {agentId} с {fromTime} по {toTime}");

            GetRamMetricsResponse result = new GetRamMetricsResponse
            {
                Metrics = new List<RamMetricDto>()
            };

            List<RamMetric> metrics = await repository.GetByTimePeriod(agentId, fromTime, toTime);

            foreach (var metric in metrics)
            {
                result.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }

            logger.LogInformation($"Отдано метрик ОЗУ агента {agentId} {result.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(result));
        }
    }
}
