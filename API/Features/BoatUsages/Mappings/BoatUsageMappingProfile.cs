using AutoMapper;

namespace API.Features.BoatUsages {

    public class BoatUsageMappingProfile : Profile {

        public BoatUsageMappingProfile() {
            CreateMap<BoatUsage, BoatUsageListVM>();
            CreateMap<BoatUsage, BoatUsageBrowserVM>();
            CreateMap<BoatUsage, BoatUsageReadDto>();
            CreateMap<BoatUsageWriteDto, BoatUsage>();
        }

    }

}