using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Responses;
using MetricsManager.DAL;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        //AllRamMetricsResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request);
        //AllHddMetricsResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request);
        AllCpuMetricsResponse GetCpuMetrics(GetAllCpuMetricsRequest request);

        AllCpuMetricsResponse GetByIdCpuMetrics(GetByIdCpuMetricsRequest request);


    }
}
