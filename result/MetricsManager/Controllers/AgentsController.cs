using AutoMapper;
using MetricsManager.DAL.Interfaces.Info;
using MetricsManager.DAL.Models;
using MetricsManager.Response;
using MetricsManager.Response.DTO;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private IAgentsInfoRepository repository;
        private readonly IMapper mapper;
        private ILogger logger;

        public AgentsController(IAgentsInfoRepository repository, IMapper mapper, ILogger<AgentsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            logger.LogInformation("Добавление нового агента");
            await repository.Create(agentInfo);
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public async Task<IActionResult> EnableAgentById([FromRoute] int agentId)
        {
            logger.LogInformation($"Активация агента {agentId}");
            await repository.Enable(agentId);
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public async Task<IActionResult> DisableAgentById([FromRoute] int agentId)
        {
            logger.LogInformation($"Деактивация агента {agentId}");
            await repository.Disable(agentId);
            return Ok();
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Запрос всех агентов");

            GetAgentsInfoResponse response = new GetAgentsInfoResponse()
            {
                Agents = new List<AgentInfoDto>()
            };

            List<AgentInfo> agents = new List<AgentInfo>();
            agents.AddRange(await repository.GetAll());

            foreach(var agent in agents)
            {
                response.Agents.Add(mapper.Map<AgentInfoDto>(agent));
            }

            logger.LogInformation($"Передано агентов {response.Agents.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }
        [HttpGet("getbyid{agentId}")]
        public async Task<IActionResult> GetById([FromRoute] int agentId)
        {
            logger.LogInformation($"Запрос агентоа {agentId}");

            GetAgentsInfoResponse response = new GetAgentsInfoResponse()
            {
                Agents = new List<AgentInfoDto>()
            };

            List<AgentInfo> agents = new List<AgentInfo>();
            agents.Add(await repository.GetById(agentId));

            foreach (var agent in agents)
            {
                response.Agents.Add(mapper.Map<AgentInfoDto>(agent));
            }

            logger.LogInformation($"Передано агентов {response.Agents.Count}");

            return Ok(JsonSerializer.Serialize(response));
        }

    }
}
