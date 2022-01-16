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
    public class HddMetricRepository : IHddMetricsRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public HddMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(HddMetric item)
        {
            if (await Exists(item))
            {
                return;
            }
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO hddmetrics(agentid, metricid, value, time) VALUES(@agentid, @metricid,@value, @time)",
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
                    "DELETE FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task<IList<HddMetric>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<HddMetric>("SELECT Id, AgentId, MetricId, Value, Time FROM hddmetrics").ToList();
            }
        }

        public async Task<HddMetric> GetById(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                HddMetric result = await connection.QueryFirstOrDefaultAsync<HddMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task<List<HddMetric>> GetByTimePeriod(int agentId, TimeSpan fromTime, TimeSpan toTime)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<HddMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM hddmetrics WHERE Time >= @fromTime AND Time <= @toTime AND AgentId = @agentId",
                    new
                    {
                        fromTime = fromTime.TotalSeconds,
                        toTime = toTime.TotalSeconds,
                        agentId = agentId
                    }).ToList();
            }
        }

        public async Task<HddMetric> GetLastValue(int agentId)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                HddMetric result = await connection.QueryFirstOrDefaultAsync<HddMetric>(
                    "SELECT Id, AgentId, MetricId, Value, Time FROM hddmetrics WHERE AgentId=@agentid AND Time=(SELECT MAX(Time) FROM hddmetrics WHERE AgentId=@agentid)",
                    new
                    {
                        agentid = agentId
                    });
                return result;
            }
        }

        public async Task Update(HddMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE hddmetrics SET value = @value, time = @time, metricid = @metricid, agentid = @agentid WHERE id=@id",
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

        public async Task<bool> Exists(HddMetric item)
        {
            HddMetric result;
            await using (var connection = new SQLiteConnection(connectionString))
            {
                result = await connection.QueryFirstOrDefaultAsync<HddMetric>("SELECT Id, AgentId, MetricId, Value, Time FROM hddmetrics WHERE AgentId=@agentid AND MetricId=@metricid AND Value=@value AND Time=@time",
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
