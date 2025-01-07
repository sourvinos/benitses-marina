using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Prices {

    public class PriceMappingProfile : Profile {

        public PriceMappingProfile() {
            CreateMap<Price, PriceListVM>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)));
            CreateMap<Price, PriceReadDto>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)));
            CreateMap<PriceWriteDto, Price>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.LongDescription, x => x.MapFrom(x => x.LongDescription.Trim()));
        }

    }

}