using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentInfoController : ControllerBase
    {
        AgentRepository repository;

        AgentInfoController(AgentRepository repository)
        {
            this.repository = repository;
        }
    }
}
