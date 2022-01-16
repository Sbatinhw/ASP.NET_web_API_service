using Dapper;
using MetricsAgent.Infrastructure.Handlers;
using MetricsManager.DAL.Interfaces.Info;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsInfoRepository : IAgentsInfoRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public AgentsInfoRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(AgentInfo item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO agentsinfo(agentadress, enable) VALUES(@agentadress, @enable)",
                    new
                    {
                        agentadress = item.AgentAdress,
                        enable = true
                    });
            }
        }

        public async Task Delete(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "DELETE FROM agentsinfo WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task Disable(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE agentsinfo SET enable = @enable WHERE agentid=@id",
                    new
                    {
                        enable = false,
                        id = id
                    });
            }
        }

        public async Task Enable(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE agentsinfo SET enable = @enable WHERE agentid=@id",
                    new
                    {
                        enable = true,
                        id = id
                    });
            }
        }

        public async Task<IList<AgentInfo>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                var result = connection.Query<AgentInfo>("SELECT agentid, agentadress FROM agentsinfo WHERE enable = true").ToList();
                return result;
            }
        }

        public async Task<AgentInfo> GetById(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                AgentInfo result = await connection.QuerySingleAsync<AgentInfo>(
                    "SELECT agentid, agentadress FROM agentsinfo WHERE agentid=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task Update(AgentInfo item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE agentsinfo SET agentadress = @agentadress WHERE agentid=@id",
                    new
                    {
                        agentadress = item.AgentAdress,
                        agentid = item.AgentId
                    });
            }
        }
    }
}
