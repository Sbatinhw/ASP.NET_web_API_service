using MetricsManager.DAL.Models;
using MetricsManager.Response.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Response
{
    public class GetAgentsInfoResponse
    {
        public List<AgentInfoDto> Agents { get; set; }
    }
}
