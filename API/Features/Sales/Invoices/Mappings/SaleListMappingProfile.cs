using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class SaleListMappingProfile : Profile {

        public SaleListMappingProfile() {
            CreateMap<Invoice, InvoiceListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.DocumentType.Id,
                    Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch
                }));
        }

    }

}