using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Cashiers.Transactions {

    public class CashierMappingProfile : Profile {

        public CashierMappingProfile() {
            // List
            CreateMap<Cashier, CashierListVM>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.CashierId.ToString()))
                .ForMember(x => x.Debit, x => x.MapFrom(x => x.Entry == "+" ? x.Amount : 0))
                .ForMember(x => x.Credit, x => x.MapFrom(x => x.Entry == "-" ? x.Amount : 0))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Company, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Company.Id,
                    Description = x.Company.Description
                }))
                .ForMember(x => x.Safe, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Safe.Id,
                    Description = x.Safe.Description
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks ?? ""))
                .ForMember(x => x.PutAt, x => x.MapFrom(x => x.PutAt.Substring(0, 10)))
                .ForMember(x => x.HasDocument, x => x.MapFrom(x => CashierHelpers.HasDocument(x)))
                .ForMember(x => x.DocumentName, x => x.MapFrom(x => CashierHelpers.DocumentName(x) ?? ""));
            // GetById
            CreateMap<Cashier, CashierReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Company, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Company.Id,
                    Description = x.Company.Description
                }))
                .ForMember(x => x.Safe, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Safe.Id,
                    Description = x.Safe.Description
                }));
            // Write cashier
            CreateMap<CashierWriteDto, Cashier>()
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks ?? ""));
        }

    }

}