using AutoMapper;

namespace API.Featuers.Sales.HullTypes {

    public class HullTypeMappingProfile : Profile {

        public HullTypeMappingProfile() {
            CreateMap<HullType, HullTypeListVM>();
            CreateMap<HullType, HullTypeBrowserVM>();
            CreateMap<HullType, HullTypeReadDto>();
            CreateMap<HullTypeWriteDto, HullType>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}