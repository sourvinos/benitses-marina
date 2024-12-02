using AutoMapper;

namespace API.Features.BoatTypes {

    public class BoatTypeMappingProfile : Profile {

        public BoatTypeMappingProfile() {
            CreateMap<BoatType, BoatTypeListVM>();
            CreateMap<BoatType, BoatTypeBrowserVM>();
            CreateMap<BoatType, BoatTypeReadDto>();
            CreateMap<BoatTypeWriteDto, BoatType>();
        }

    }

}