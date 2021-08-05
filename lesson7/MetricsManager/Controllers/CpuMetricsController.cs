using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using System.Net.Http;
using System.Net;
using Grpc.Net.ClientFactory;
using System.Text.Json;
using MetricsManager.DAL;
using MetricsManager.Models;
using AutoMapper;
using MetricsManager.Responses;
using MetricsManager.Client;
using Polly;


namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private IMetricsAgentClient metricsAgentClient;
        private readonly ICpuMetricsRepository repository;
        private readonly IMapper mapper;
        private ILogger logger;

        public CpuMetricsController(
            ICpuMetricsRepository repository, 
            IMapper mapper, 
            IMetricsAgentClient metricsAgentClient)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.metricsAgentClient = metricsAgentClient;
        }

        [HttpPost("create")]
        public IActionResult Create ([FromBody] CpuMetricsCreateRequest request)
        {
            repository.Create(new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
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

            return Ok(response);

        }



        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent
            (
            [FromRoute] int agentId, 
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime
            )
        {
            logger.LogInformation("Запрос метрик");

            var metrics = repository.GetClusterId(fromTime.TotalSeconds, toTime.TotalSeconds, agentId);

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }



        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster
            (
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime
            )
        {
            logger.LogInformation("Запрос метрик");

            var metrics = repository.GetCluster(fromTime.TotalSeconds, toTime.TotalSeconds);

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }


    }
}
