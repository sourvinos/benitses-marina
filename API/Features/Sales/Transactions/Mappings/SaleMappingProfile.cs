using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Transactions {

    public class SaleMappingProfile : Profile {

        public SaleMappingProfile() {
            // List
            CreateMap<Sale, SaleListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity { Id = x.DocumentType.Id, Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new SaleListAadeVM { Mark = x.Aade.Mark != "", MarkCancel = x.Aade.MarkCancel != "" }));
            // GetById
            CreateMap<Sale, SaleReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.TripDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.TripDate)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SaleFormDocumentTypeVM {
                    Id = x.DocumentType.Id,
                    Abbreviation = x.DocumentType.AbbreviationEn,
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new SaleFormAadeVM {
                    InvoiceId = x.Aade.InvoiceId,
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    QrUrl = x.Aade.QrUrl
                }));
            // Create invoice
            CreateMap<SaleCreateDto, Sale>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 1))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new SaleAade {
                    InvoiceId = x.InvoiceId,
                    Uid = "",
                    Mark = "",
                    MarkCancel = "",
                    QrUrl = ""
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()));
            // Update invoice
            CreateMap<InvoiceUpdateDto, Sale>();
            // Update aade
            CreateMap<InvoiceUpdateDto, SaleAade>();
        }

    }

}