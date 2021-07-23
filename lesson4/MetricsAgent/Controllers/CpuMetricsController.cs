using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository repository;

        private readonly IMapper mapper;

        public CpuMetricsController(ICpuMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create ([FromBody] CpuMetricCreateRequest request)
        {
            repository.Create(new CpuMetric { Time = request.Time, Value = request.Value });
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach(var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<CpuMetricDto>(metric));
            }
            return Ok(response);
        }

    }

    //это в удаление
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController_Test : ControllerBase
    {
        [HttpGet("sql-read-write-test")]
        public IActionResult TryToInsertAndRead()
        {
            string connectionString = "Data Source=:memory:";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var comand = new SQLiteCommand(connection))
                {
                    comand.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                    comand.ExecuteNonQuery();

                    comand.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
                    comand.ExecuteNonQuery();

                    comand.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(10,1)";
                    comand.ExecuteNonQuery();
                    comand.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(50,2)";
                    comand.ExecuteNonQuery();
                    comand.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(75,4)";
                    comand.ExecuteNonQuery();
                    comand.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(90,5)";
                    comand.ExecuteNonQuery();

                    string readQuery = "SELECT * FROM cpumetrics LIMIT 3";

                    var returnArray = new CpuMetric[3];

                    comand.CommandText = readQuery;

                    using (SQLiteDataReader reader = comand.ExecuteReader())
                    {
                        var counter = 0;

                        while (reader.Read())
                        {
                            returnArray[counter] = new CpuMetric
                            {
                                ID = reader.GetInt32(0),
                                Value = reader.GetInt32(1),
                                //Time = reader.GetInt64(2)
                            };
                            counter++;
                        }
                    }
                    return Ok(returnArray);


                }

            }
        }

        [HttpGet("sql-test")]
        public IActionResult TryToSqlite()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();

                using var cmd = new SQLiteCommand(stm, con);
                string version = cmd.ExecuteScalar().ToString();

                return Ok(version);
            }
        }




        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent
            (
            [FromRoute] int agentId, 
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime
            )
        {
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster
            (
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime
            )
        {
            return Ok();
        }


    }
}
