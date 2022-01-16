using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces.Metrics
{
    public interface IMetricsRepository<T> where T : class
    {
        Task Create(T item);
        Task Delete(int id);
        Task Update(T item);
        Task<T> GetLastValue(int agentId);
        Task<T> GetById(int id);
        Task<List<T>> GetByTimePeriod(int agentId, TimeSpan fromTime, TimeSpan toTime);
        Task<IList<T>> GetAll();
        Task<bool> Exists(T item);
    }
}
