using API.Features.Expenses.Transactions;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Expenses.BalanceSheet {

    public class BalanceSheetMappingProfile : Profile {

        public BalanceSheetMappingProfile() {
            CreateMap<TransactionsBase, BalanceSheetVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Supplier, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Supplier.Id,
                    Description = x.Supplier.Description
                }))
                .ForMember(x => x.Debit, x => x.MapFrom(source => source.DocumentType.Suppliers == "-" ? source.Amount : 0))
                .ForMember(x => x.Credit, x => x.MapFrom(source => source.DocumentType.Suppliers == "+" ? source.Amount : 0));
        }

    }

}