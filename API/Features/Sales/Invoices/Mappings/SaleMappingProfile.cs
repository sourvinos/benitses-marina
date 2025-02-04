using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class SaleMappingProfile : Profile {

        public SaleMappingProfile() {
            // List
            CreateMap<Invoice, InvoiceListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity { Id = x.DocumentType.Id, Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch }));
            // .ForMember(x => x.Aade, x => x.MapFrom(x => new SaleListAadeVM { Mark = x.Aade.Mark != "", MarkCancel = x.Aade.MarkCancel != "" }));
            // Read
            CreateMap<Invoice, InvoiceReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeVM {
                    Id = x.DocumentType.Id,
                    Abbreviation = x.DocumentType.Abbreviation,
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }))
                // .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceFormAadeVM {
                //     InvoiceId = x.Aade.InvoiceId,
                //     UId = x.Aade.Uid,
                //     Mark = x.Aade.Mark,
                //     MarkCancel = x.Aade.MarkCancel,
                //     QrUrl = x.Aade.QrUrl
                // }))
                .ForMember(x => x.Items, x => x.MapFrom(x => x.Items.Select(x => new InvoiceItemReadDto {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId.ToString(),
                    Code = x.Code,
                    Description = x.Description,
                    EnglishDescription = x.EnglishDescription,
                    Remarks = x.Remarks,
                    Qty = x.Quantity,
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.VatAmount,
                    GrossAmount = x.GrossAmount
                })));
            // Write invoice
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 1))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.Items, x => x.MapFrom(x => x.Items.Select(x => new InvoiceItemWriteDto {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    Code = x.Code,
                    Description = x.Description,
                    EnglishDescription = x.EnglishDescription,
                    Remarks = x.Remarks,
                    Quantity = x.Quantity,
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.VatAmount,
                    GrossAmount = x.GrossAmount
                })));
            // Write item
            CreateMap<InvoiceItemWriteDto, InvoiceItem>();
            // .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceAade {
            //     InvoiceId = x.InvoiceId,
            //     Uid = "",
            //     Mark = "",
            //     MarkCancel = "",
            //     QrUrl = ""
            // }))
        }

    }

}