using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using AutoMapper;
using MetricsAgent.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<CpuMetricsController> logger;

        public CpuMetricsController(
            ICpuMetricsRepository repository, 
            IMapper mapper,
            ILogger<CpuMetricsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create ([FromBody] CpuMetricCreateRequest request)
        {
            repository.Create(new CpuMetric { Time = request.Time, Value = request.Value });

            Console.WriteLine($"{DateTime.Now} CPU Добавлена метрика: {request.Time} {request.Value}");
            logger.LogInformation($"CPU Добавлена метрика: {request.Time} {request.Value}");

            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetFromCluster(
            [FromRoute] long fromTime,
            [FromRoute] long toTime)
        {
            var metrics = repository.GetCluster(fromTime, toTime);

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach(var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} CPU Отдано: {response.Metrics.Count}");
            logger.LogInformation($"CPU Диапазон: {fromTime} - {toTime} Отдано метрик: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach(var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} CPU Отдано: {response.Metrics.Count}");
            logger.LogInformation($"CPU Все метрики. Отдано: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize( response));
        }

    }

    
}
