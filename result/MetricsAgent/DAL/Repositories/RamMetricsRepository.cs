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
    public class RamMetricsRepository: IRamMetricsRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public RamMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(RamMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
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
                    "DELETE FROM rammetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task<RamMetric> GetById(int id)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                RamMetric result = await connection.QuerySingleAsync<RamMetric>(
                    "SELECT Id, Time, Value FROM rammetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task<List<RamMetric>> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<RamMetric>(
                    "SELECT Id, Time, Value FROM rammetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime.TotalSeconds,
                        toTime = toTime.TotalSeconds
                    }).ToList();
            }

        }

        public async Task<IList<RamMetric>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<RamMetric>("SELECT Id, Time, Value FROM rammetrics").ToList();
            }
        }

        public async Task Update(RamMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE rammetrics SET value = @value, time = @time WHERE id=@id",
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
