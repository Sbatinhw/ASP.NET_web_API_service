using Dapper;
using MetricsAgent.Infrastructure.Handlers;
using MetricsManager.DAL.Interfaces.Metrics;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class RamMetricRepository: IRamMetricsRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public RamMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(RamMetric item)
        {
            if (await Exists(item))
            {
                return;
            }
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO rammetrics(agentid, metricid, value, time) VALUES(@agentid, @metricid,@value, @time)",
                    new
                    {
                        agentid = item.AgentId,
                        metricid = item.MetricId,
                        value = item.Value,
                        time = item.Time.TotalSeconds
                    });
            }
        }

        public async Task Delete(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "DELETE FROM rammetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task<IList<RamMetric>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<RamMetric>("SELECT Id, AgentId, MetricId, Value, Time FROM rammetrics").ToList();
            }
        }

        public async Task<RamMetric> GetById(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                RamMetric result = await connection.QueryFirstOrDefaultAsync<RamMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM rammetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task<List<RamMetric>> GetByTimePeriod(int agentId, TimeSpan fromTime, TimeSpan toTime)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<RamMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM rammetrics WHERE Time >= @fromTime AND Time <= @toTime AND AgentId = @agentId",
                    new
                    {
                        fromTime = fromTime.TotalSeconds,
                        toTime = toTime.TotalSeconds,
                        agentId = agentId
                    }).ToList();
            }
        }

        public async Task<RamMetric> GetLastValue(int agentId)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                RamMetric result = await connection.QueryFirstOrDefaultAsync<RamMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM rammetrics WHERE AgentId=@agentid AND Time=(SELECT MAX(Time) FROM rammetrics WHERE AgentId=@agentid)",
                    new
                    {
                        agentid = agentId
                    });
                return result;
            }
        }

        public async Task Update(RamMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE rammetrics SET value = @value, time = @time, metricid = @metricid, agentid = @agentid WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id,
                        metricid = item.MetricId,
                        agentid = item.AgentId
                    });
            }
        }

        public async Task<bool> Exists(RamMetric item)
        {
            RamMetric result;
            await using (var connection = new SQLiteConnection(connectionString))
            {
                result = await connection.QueryFirstOrDefaultAsync<RamMetric>("SELECT Id, AgentId, MetricId, Value, Time FROM rammetrics WHERE AgentId=@agentid AND MetricId=@metricid AND Value=@value AND Time=@time",
                    new
                    {
                        agentid = item.AgentId,
                        metricid = item.MetricId,
                        value = item.Value,
                        time = item.Time.TotalSeconds
                    });
            }

            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
