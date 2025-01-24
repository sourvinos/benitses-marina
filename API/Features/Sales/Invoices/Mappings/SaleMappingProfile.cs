using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class SaleMappingProfile : Profile {

        public SaleMappingProfile() {
            CreateMap<Invoice, InvoiceistVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity { Id = x.DocumentType.Id, Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new SaleListAadeVM { Mark = x.Aade.Mark != "", MarkCancel = x.Aade.MarkCancel != "" }));
        }

    }

}