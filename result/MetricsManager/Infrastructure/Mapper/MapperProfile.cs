using AutoMapper;
using MetricsManager.DAL.Models;
using MetricsManager.Response.DTO;
using MetricsManager.Response.DTO.AgentMetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Infrastructure.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<AgentInfo, AgentInfoDto>();

            CreateMap<CpuMetric, CpuMetricDto>().ForMember(destinationMember =>
                destinationMember.Time, memberOptions =>
                    memberOptions.MapFrom(sourceMember =>
                        sourceMember.Time.TotalSeconds));
            CreateMap<HddMetric, HddMetricDto>().ForMember(destinationMember =>
                destinationMember.Time, memberOptions =>
                    memberOptions.MapFrom(sourceMember =>
                        sourceMember.Time.TotalSeconds));
            CreateMap<RamMetric, RamMetricDto>().ForMember(destinationMember =>
                destinationMember.Time, memberOptions =>
                    memberOptions.MapFrom(sourceMember =>
                        sourceMember.Time.TotalSeconds));
        }
    }
}
