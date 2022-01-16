using AutoMapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;
using MetricsAgent.Responses.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
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

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public async Task<IActionResult> GetMetricsByTimePeriod(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            logger.LogInformation($"Запрос метрик ПЗУ с {fromTime} по {toTime}");

            GetHddMetricsResponse result = new GetHddMetricsResponse
            {
                Metrics = new List<HddMetricDto>()
            };

            List<HddMetric> metrics = await repository.GetByTimePeriod(fromTime, toTime);

            foreach (var metric in metrics)
            {
                result.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }

            logger.LogInformation($"Отдано метрик ПЗУ {result.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(result));
        }
    }
}
