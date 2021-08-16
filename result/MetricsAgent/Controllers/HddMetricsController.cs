using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Models;
using MetricsAgent.DAL;
using AutoMapper;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using System.Text.Json;
using Microsoft.Extensions.Logging;


namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<HddMetricsController> logger;

        public HddMetricsController(
            IHddMetricRepository repository, 
            IMapper mapper,
            ILogger<HddMetricsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            repository.Create(new HddMetric { Time = request.Time, Value = request.Value });

            Console.WriteLine($"Добавлена метрика HDD: {request.Time} {request.Value}");
            logger.LogInformation($"Добавлена метрика HDD: {request.Time} {request.Value}");

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} HDD Отдано: {response.Metrics.Count}");
            logger.LogInformation($"HDD Все метрики. Отдано: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetFromCluster(
            [FromRoute] long fromTime,
            [FromRoute] long toTime)
        {
            var metrics = repository.GetCluster(fromTime, toTime);

            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} HDD Отдано: {response.Metrics.Count}");
            logger.LogInformation($"HDD Диапазон: {fromTime} - {toTime} Отдано метрик: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

    }


    
}
