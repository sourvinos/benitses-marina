using API.Features.Cashiers.Transactions;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Cashiers.Ledgers {

    public class CashierLedgerMappingProfile : Profile {

        public CashierLedgerMappingProfile() {
            CreateMap<Cashier, CashierLedgerVM>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.CashierId.ToString()))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks))
                .ForMember(x => x.Debit, x => x.MapFrom(x => x.Entry == "+" ? x.Amount : 0))
                .ForMember(x => x.Credit, x => x.MapFrom(x => x.Entry == "-" ? x.Amount : 0));
        }

    }

}