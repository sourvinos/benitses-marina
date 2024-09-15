using AutoMapper;

namespace API.Features.Reservations.Piers {

    public class PierMappingProfile : Profile {

        public PierMappingProfile() {
            CreateMap<Pier, PierListVM>();
            CreateMap<Pier, PierBrowserVM>();
            CreateMap<Pier, PierReadDto>();
            CreateMap<PierWriteDto, Pier>();
        }

    }

}