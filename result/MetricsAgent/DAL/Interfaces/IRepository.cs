using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Create(T item);
        Task Delete(int id);
        Task Update(T item);
        Task<T> GetById(int id);
        Task<List<T>> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
        Task<IList<T>> GetAll();
    }
}
