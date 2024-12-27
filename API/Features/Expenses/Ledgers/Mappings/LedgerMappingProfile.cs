using API.Features.Expenses.Invoices;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Expenses.Ledgers {

    public class LedgerMappingProfile : Profile {

        public LedgerMappingProfile() {
            CreateMap<Invoice, LedgerVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Supplier, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Supplier.Id,
                    Description = x.Supplier.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeLedgerVM {
                    Id = x.DocumentType.Id,
                    Description = x.DocumentType.Description,
                    Customers = x.DocumentType.Customers,
                    Suppliers = x.DocumentType.Suppliers
                }))
                .ForMember(x => x.InvoiceNo, x => x.MapFrom(x => x.DocumentNo.ToString()))
                .ForMember(x => x.Debit, x => x.MapFrom(x => x.DocumentType.Customers == "+" || x.DocumentType.Suppliers == "-" ? x.Amount : 0))
                .ForMember(x => x.Credit, x => x.MapFrom(x => x.DocumentType.Customers == "-" || x.DocumentType.Suppliers == "+" ? x.Amount : 0));
        }

    }

}