using Dapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.Infrastructure.Handlers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public HddMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(HddMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO hddmetrics(value, time) VALUES(@value, @time)",
                    new
                    {
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

        public async Task<HddMetric> GetById(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                HddMetric result = await connection.QuerySingleAsync<HddMetric>(
                    "SELECT Id, Time, Value FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task<List<HddMetric>> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<HddMetric>(
                    "SELECT Id, Time, Value FROM hddmetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime.TotalSeconds,
                        toTime = toTime.TotalSeconds
                    }).ToList();
            }

        }

        public async Task<IList<HddMetric>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<HddMetric>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }

        public async Task Update(HddMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                    });
            }
        }
    }
}
