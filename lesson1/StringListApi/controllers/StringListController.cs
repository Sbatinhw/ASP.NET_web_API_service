using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StringListApi
{
    [ApiController]
    [Route("[controller]")]

    public class StringListController : ControllerBase
    {
        ValuesHolder holder;

        //работает
        public StringListController(ValuesHolder str)
        {
            holder = str;
        }

        /// <summary>
        /// возвращает всё содержимое
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return string.Join("\n", holder.list);
        }

        /// <summary>
        /// создаёт новый элемент
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult create([FromQuery] string val)
        {
            holder.list.Add(val);
            return Ok();
        }

        /// <summary>
        /// возвращает значение по указаному индексу
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpGet("read")]
        public string Get([FromQuery]int val)
        {
            if (val < holder.list.Count)
            {
                return holder.list[val];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// удаляет все элементы
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deleteall")]
        public IActionResult deleteAll()
        {
            holder.list = new List<string>();
            return Ok();
        }

        //
    }
}
