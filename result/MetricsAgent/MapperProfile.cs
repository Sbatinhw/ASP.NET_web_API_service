using AutoMapper;
using MetricsAgent.Responses;
using MetricsAgent.Models;




namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
        }
    }
}
