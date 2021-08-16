using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();

        IList<T> GetCluster(double fromTime, double toTime);
        IList<T> GetClusterId(double fromTime, double toTime, int agentId);


        T GetByID(int id);

        void Create(T item, int agentIs);
        void Create(T item);
        void Update(T item);
        void Delete(int id);

    }
}
