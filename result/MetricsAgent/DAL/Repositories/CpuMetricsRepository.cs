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
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public async Task Create(CpuMetric item)
        {
            await using(var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds
                    });
            }
        }

        public async Task Delete(int id)
        {
            await using(var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "DELETE FROM cpumetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task<CpuMetric> GetById(int id)
        {
            await using(var connection = new SQLiteConnection(connectionString))
            {
                CpuMetric result = await connection.QuerySingleAsync<CpuMetric>(
                    "SELECT Id, Time, Value FROM cpumetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
                return result;
            }
        }

        public async Task<List<CpuMetric>> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<CpuMetric>(
                    "SELECT Id, Time, Value FROM cpumetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new {
                        fromTime = fromTime.TotalSeconds,
                        toTime = toTime.TotalSeconds
                    }).ToList();
            }

        }

        public async Task<IList<CpuMetric>> GetAll()
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                return connection.Query<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }
        }

        public async Task Update(CpuMetric item)
        {
            await using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync("UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id",
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
