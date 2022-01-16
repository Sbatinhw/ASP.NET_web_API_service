using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces.Info
{
    public interface IAgentsInfoRepository
    {
        Task Create(AgentInfo item);
        Task Delete(int id);
        Task Enable(int id);
        Task Disable(int id);
        Task Update(AgentInfo item);
        Task<AgentInfo> GetById(int id);
        Task<IList<AgentInfo>> GetAll();
    }
}
