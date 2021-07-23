using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class AllAgentInfoResponse
    {
        public List<AgentInfoDto> Agents { get; set; }
    }

    public class AgentInfoDto
    {
        public int AgentID { get; set; }
        public Uri AgentAdress { get; set; }
    }


}
