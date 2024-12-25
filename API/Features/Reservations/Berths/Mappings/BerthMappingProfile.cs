using AutoMapper;

namespace API.Features.Reservations.Berths {

    public class BerthMappingProfile : Profile {

        public BerthMappingProfile() {
            CreateMap<Berth, BerthListVM>();
            CreateMap<Berth, BerthBrowserVM>();
            CreateMap<Berth, BerthReadDto>();
            CreateMap<BerthWriteDto, Berth>();
        }

    }

}