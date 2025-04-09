using AutoMapper;

namespace API.Featuers.Sales.SeasonTypes {

    public class SeasonTypeMappingProfile : Profile {

        public SeasonTypeMappingProfile() {
            CreateMap<SeasonType, SeasonTypeListVM>();
            CreateMap<SeasonType, SeasonTypeBrowserVM>();
            CreateMap<SeasonType, SeasonTypeReadDto>();
            CreateMap<SeasonTypeWriteDto, SeasonType>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}