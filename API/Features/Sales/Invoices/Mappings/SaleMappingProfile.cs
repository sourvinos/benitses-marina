using System.Linq;
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
            CreateMap<Invoice, InvoiceReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeVM {
                    Id = x.DocumentType.Id,
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceFormAadeVM {
                    InvoiceId = x.Aade.InvoiceId,
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    QrUrl = x.Aade.QrUrl
                }))
                .ForMember(x => x.InvoiceLineItems, x => x.MapFrom(x => x.LineItems.Select(x => new InvoiceLineItemReadDto {
                    // Id = x.Id,
                    // InvoiceId = x.InvoiceId,
                    GrossAmount = x.GrossAmount
                })));
            // Create invoice
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 1))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceAade {
                    InvoiceId = x.InvoiceId,
                    Uid = "",
                    Mark = "",
                    MarkCancel = "",
                    QrUrl = ""
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()));
        }

    }

}