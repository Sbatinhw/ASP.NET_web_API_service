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

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public async Task<IActionResult> GetMetricsByTimePeriod(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            logger.LogInformation($"Запрос метрик ОЗУ с {fromTime} по {toTime}");

            GetRamMetricsResponse result = new GetRamMetricsResponse
            {
                Metrics = new List<RamMetricDto>()
            };

            List<RamMetric> metrics = await repository.GetByTimePeriod(fromTime, toTime);

            foreach (var metric in metrics)
            {
                result.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }

            logger.LogInformation($"Отдано метрик ОЗУ {result.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(result));
        }
    }
}
