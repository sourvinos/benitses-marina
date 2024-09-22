using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.BoatTypes {

    public class BoatTypeMappingProfile : Profile {

        public BoatTypeMappingProfile() {
            CreateMap<BoatType, BoatTypeListVM>();
            CreateMap<BoatType, BoatTypeBrowserVM>();
            CreateMap<BoatType, SimpleEntity>();
            CreateMap<BoatType, BoatTypeReadDto>();
            CreateMap<BoatTypeWriteDto, BoatType>().ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}