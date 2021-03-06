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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository repository;
        private readonly IMapper mapper;
        private ILogger logger;

        public CpuMetricsController(ICpuMetricsRepository repository, IMapper mapper, ILogger<CpuMetricsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public async Task<IActionResult> GetMetricsByTimePeriod (
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            logger.LogInformation($"Запрос метрик ЦПУ с {fromTime} по {toTime}");

            GetCpuMetricsResponse result = new GetCpuMetricsResponse
            {
                Metrics = new List<CpuMetricDto>()
            };

            List<CpuMetric> metrics = await repository.GetByTimePeriod(fromTime, toTime);

            foreach(var metric in metrics)
            {
                result.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }

            logger.LogInformation($"Отдано метрик ЦПУ {result.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(result));
        }
    }
}
