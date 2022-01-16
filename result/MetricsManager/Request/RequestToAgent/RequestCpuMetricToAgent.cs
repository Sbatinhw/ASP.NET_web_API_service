using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Request.RequestToAgent
{
    public class RequestCpuMetricToAgent
    {
        public AgentInfo Agent { get; set; }
        public TimeSpan fromTime { get; set; }
        public TimeSpan toTime { get; set; }
        public string ConnectionLine { get { return $"{Agent.AgentAdress}/api/metrics/cpu/cluster/from/{fromTime}/to/{toTime}"; } }
    }
}
