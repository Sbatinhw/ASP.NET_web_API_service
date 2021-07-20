using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public interface IAgentsInfoRepository : IRepository<AgentInfo>
    {

    }
    public class AgentsInfoRepository : IAgentsInfoRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public void Create(AgentInfo item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "INSERT INTO agentinfo(agentId, agentAdress) VALUES(@agentId, @agentAdress)";
            cmd.Parameters.AddWithValue("@agentId", item.AgentID);
            cmd.Parameters.AddWithValue("@agentAdress", item.AgentAdress);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "DELETE FROM agentinfo WHERE agentId=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

        }

        public void Update(AgentInfo item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "UPDATE agentinfo SET agentAdress = @agentAdress WHERE agentId=@id;";
            cmd.Parameters.AddWithValue("@id", item.AgentID);
            cmd.Parameters.AddWithValue("@agentAdress", item.AgentAdress);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<AgentInfo> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);

            cmd.CommandText = "SELECT * FROM agentinfo";

            var returnList = new List<AgentInfo>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new AgentInfo
                    {
                        AgentID = reader.GetInt32(0),
                        AgentAdress = new Uri(reader.GetString(1))
                    }) ;
                    
                }
            }

            return returnList;
        }        

        public AgentInfo GetByID(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM agentinfo WHERE agentId=@id";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new AgentInfo
                    {
                        AgentID = reader.GetInt32(0),
                        AgentAdress = new Uri(reader.GetString(1))
                    };
                }
                else
                {
                    return null;
                }
            }
        }


        //
    }
}
