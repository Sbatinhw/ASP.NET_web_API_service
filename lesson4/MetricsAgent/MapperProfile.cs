using AutoMapper;
using MetricsAgent.Responses;




namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<AgentInfo, AgentInfoDto>();
        }
    }
}
