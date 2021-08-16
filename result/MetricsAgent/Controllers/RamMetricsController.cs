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
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<RamMetricsController> logger;


        public RamMetricsController(
            IRamMetricRepository repository, 
            IMapper mapper,
            ILogger<RamMetricsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            repository.Create(new RamMetric { Time = request.Time, Value = request.Value });

            Console.WriteLine($"{DateTime.Now} RAM Добавлена метрика: {request.Time} {request.Value}");
            logger.LogInformation($"RAM Добавлена метрика: {request.Time} {request.Value}");

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} RAM Отдано: {response.Metrics.Count}");
            logger.LogInformation($"RAM Все метрики. Отдано: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetFromCluster(
            [FromRoute] long fromTime,
            [FromRoute] long toTime)
        {
            var metrics = repository.GetCluster(fromTime, toTime);

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }

            Console.WriteLine($"{DateTime.Now} RAM Отдано: {response.Metrics.Count}");
            logger.LogInformation($"RAM Диапазон: {fromTime} - {toTime} Отдано метрик: {response.Metrics.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

    }

}
