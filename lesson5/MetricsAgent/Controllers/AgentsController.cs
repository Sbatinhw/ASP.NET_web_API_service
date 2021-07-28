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

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        //задание к 4 уроку
        private IAgentsInfoRepository repository;

        private readonly IMapper mapper;

        public AgentsController(IAgentsInfoRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var agents = repository.GetAll();

            var response = new AllAgentInfoResponse()
            {
                Agents = new List<AgentInfoDto>()
            };

            foreach (var agent in agents)
            {
                response.Agents.Add(mapper.Map<AgentInfoDto>(agents));
            }
            return Ok(response);
        }
        //конец задания

        [HttpGet("allregistered")]
        public IActionResult AllRegistered()
        {
            return Ok();
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

    }
}
