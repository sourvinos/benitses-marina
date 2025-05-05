using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Sales.Prices {

    public class PriceMappingProfile : Profile {

        public PriceMappingProfile() {
            CreateMap<Price, PriceListVM>()
                .ForMember(x => x.HullType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.HullType.Id,
                    Description = x.HullType.Description
                }))
                .ForMember(x => x.PeriodType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PeriodType.Id,
                    Description = x.PeriodType.Description
                }))
                .ForMember(x => x.SeasonType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.SeasonType.Id,
                    Description = x.SeasonType.Description
                }))
                .ForMember(x => x.VatAmount, x => x.MapFrom(x => (x.NetAmount * x.VatPercent / 100).ToString("F")))
                .ForMember(x => x.GrossAmount, x => x.MapFrom(x => (x.NetAmount * x.VatPercent / 100 + x.NetAmount).ToString("F")));
            CreateMap<Price, PriceListBrowserVM>();
            CreateMap<Price, PriceReadDto>()
                .ForMember(x => x.HullType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.HullType.Id,
                    Description = x.HullType.Description
                }))
                .ForMember(x => x.PeriodType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PeriodType.Id,
                    Description = x.PeriodType.Description
                }))
                .ForMember(x => x.SeasonType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.SeasonType.Id,
                    Description = x.SeasonType.Description
                }))
                .ForMember(x => x.VatAmount, x => x.MapFrom(x => (x.NetAmount * x.VatPercent / 100).ToString("F")))
                .ForMember(x => x.GrossAmount, x => x.MapFrom(x => (x.NetAmount * x.VatPercent / 100 + x.NetAmount).ToString("F")));
            CreateMap<PriceWriteDto, Price>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.EnglishDescription, x => x.MapFrom(x => x.EnglishDescription.Trim()));
        }

    }

}