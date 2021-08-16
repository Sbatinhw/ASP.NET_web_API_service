using AutoMapper;
using MetricsManager.Responses;
using MetricsManager.Models;




namespace MetricsManager
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();

            CreateMap<CpuMetricDto, CpuMetric>();

        }
    }
}
