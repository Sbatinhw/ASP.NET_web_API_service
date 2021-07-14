using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi
{
    [Route("[controller]")]
    [ApiController]
    public class CrudWeatherController : ControllerBase
    {
        ValueHolder holder;

        public CrudWeatherController(ValueHolder _list)
        {
            holder = _list;

        }

        [HttpPost("create")]
        public IActionResult Create(DateTime dateTime)
        {
            holder.list.Add(new WeatherForecast(dateTime));
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update(DateTime dateTime, int value)
        {
            for(int i = 0; i < holder.list.Count; i++)
            {
                if(holder.list[i].Date == dateTime)
                {
                    holder.list[i].TemperatureC = value;
                    break;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete(DateTime start, DateTime end)
        {
            holder.list = holder.list.Where(w => w.Date < start || w.Date > end).ToList();
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read(DateTime start, DateTime end)
        {
            return Ok(holder.list.Where(w => w.Date >= start && w.Date <= end).ToList());
        }

        [HttpGet("readall")]
        public IActionResult ReadAll()
        {
            return Ok(holder.list);
        }



    }
}
