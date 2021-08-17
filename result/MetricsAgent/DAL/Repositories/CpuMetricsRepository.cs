﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using MetricsAgent.Models;

namespace MetricsAgent.DAL
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(CpuMetric item)
        {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                        new
                        {
                            value = item.Value,
                            time = item.Time
                        });
                }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM cpumetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(CpuMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time,
                        id = item.ID
                    });
            }
        }

        public IList<CpuMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }
        }





        public CpuMetric GetByID(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id=@id",
                    new { id = id });
            }
        }

        public IList<CpuMetric> GetCluster(double fromTime, double toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT id, value, time FROM cpumetrics WHERE time>@fromTime AND time<@toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }

    }
}
