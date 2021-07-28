using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.DAL
{
    public class AgentsInfoRepository : IAgentsInfoRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public AgentsInfoRepository()
        {
            
        }


        public void Create(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agentsinfo(agentId, agentAdress) VALUES(@agentId, @agentAdress)",
                    new
                    {
                        agentId = item.AgentID,
                        agentAdress = item.AgentAdress
                    });
            }

        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM agentsinfo WHERE agentId=@id",
                    new
                    {
                        agentId = id
                    });
            }
        }

        public void Update(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE agentsinfo SET agentAdress = @agentAdress WHERE agentId=@id",
                    new
                    {
                        agentId = item.AgentID,
                        agentAdress = item.AgentAdress
                    });
            }
        }

        public IList<AgentInfo> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentInfo>("SELECT agentId, agentAdress FROM agentsinfo").ToList();
            }
        }        

        public AgentInfo GetByID(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<AgentInfo>("SELECT agentId, agentAdress FROM agentsinfo WHERE agentId=@id",
                    new { id = id });
            }
        }


        //
    }
}
