using AutoMapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Infrastructure.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
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
