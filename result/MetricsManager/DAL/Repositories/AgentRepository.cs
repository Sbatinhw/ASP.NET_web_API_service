using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager;
using Dapper;
using System.Data.SQLite;

namespace MetricsManager.DAL
{
    public class AgentRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public void Create(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agents(agentadress) VALUES(agentadress)",
                    new
                    {
                        agentadress = item.AgentAdress
                    });
            }
        }

        public IList<AgentInfo> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentInfo>("SELECT agentid, agentadress FROM agents").ToList();
            }
        }

    }
}
