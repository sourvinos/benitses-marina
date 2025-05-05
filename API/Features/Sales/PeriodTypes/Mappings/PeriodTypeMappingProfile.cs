using AutoMapper;

namespace API.Featuers.Sales.PeriodTypes {

    public class PeriodTypeMappingProfile : Profile {

        public PeriodTypeMappingProfile() {
            CreateMap<PeriodType, PeriodTypeListVM>();
            CreateMap<PeriodType, PeriodTypeBrowserVM>();
            CreateMap<PeriodType, PeriodTypeReadDto>();
            CreateMap<PeriodTypeWriteDto, PeriodType>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}