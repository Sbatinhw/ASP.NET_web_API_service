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

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricRepository repository;

        private readonly IMapper mapper;

        public HddMetricsController(IHddMetricRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            repository.Create(new HddMetric { Time = request.Time, Value = request.Value });
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
            return Ok(response);
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

            return Ok(response);
        }

    }


    //
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController_test : ControllerBase
    {

        [HttpGet("left/from/{fromTime}/to/{toTime}")]
        public IActionResult left
            (
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime
            )
        {
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent
            (
            [FromRoute] int agentId,
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime
            )
        {
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster
            (
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime
            )
        {
            return Ok();
        }


    }
}
