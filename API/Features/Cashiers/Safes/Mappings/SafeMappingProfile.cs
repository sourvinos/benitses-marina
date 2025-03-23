using AutoMapper;

namespace API.Features.Cashiers.Safes {

    public class SafeMappingProfile : Profile {

        public SafeMappingProfile() {
            CreateMap<Safe, SafeListVM>();
            CreateMap<Safe, SafeBrowserVM>();
            CreateMap<Safe, SafeReadDto>();
            CreateMap<SafeWriteDto, Safe>();
        }

    }

}