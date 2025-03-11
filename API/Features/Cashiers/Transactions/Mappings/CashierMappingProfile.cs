using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Cashiers.Transactions {

    public class CashierMappingProfile : Profile {

        public CashierMappingProfile() {
            // List
            CreateMap<Cashier, CashierListVM>()
                .ForMember(x => x.CashierId, x => x.MapFrom(x => x.CashierId.ToString()))
                .ForMember(x => x.IsDebit, x => x.MapFrom(x => x.DiscriminatorId == 1))
                .ForMember(x => x.IsCredit, x => x.MapFrom(x => x.DiscriminatorId == 2))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Company, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Company.Id,
                    Description = x.Company.Description
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks ?? ""))
                .ForMember(x => x.PutAt, x => x.MapFrom(x => x.PutAt.Substring(0, 10)));
            // GetById
            CreateMap<Cashier, CashierReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)));
            // Write reservation
            CreateMap<CashierWriteDto, Cashier>()
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks ?? ""));
        }

    }

}