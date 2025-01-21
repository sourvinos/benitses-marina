using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Transactions {

    public class SalePdfMappingProfile : Profile {

        public SalePdfMappingProfile() {
            CreateMap<Sale, SalePdfVM>()
                .ForMember(x => x.Header, x => x.MapFrom(x => new SalePdfHeaderVM {
                    Date = DateHelpers.DateToISOString(x.Date),
                    TripDate = DateHelpers.DateToISOString(x.TripDate),
                    DocumentType = new SalePdfDocumentTypeVM {
                        Description = x.DocumentType.Description,
                        Batch = x.DocumentType.Batch
                    },
                    InvoiceNo = x.InvoiceNo
                }))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SalePdfPartyVM {
                    Id = x.Customer.Id,
                    FullDescription = x.Customer.FullDescription,
                    VatNumber = x.Customer.VatNumber,
                    Branch = x.Customer.Branch,
                    Profession = x.Customer.Profession,
                    Street = x.Customer.Street,
                    Number = x.Customer.Number,
                    PostalCode = x.Customer.PostalCode,
                    City = x.Customer.City,
                    Phones = x.Customer.Phones,
                    Email = x.Customer.Email,
                    Nationality = x.Customer.Nationality.Description,
                    TaxOffice = x.Customer.TaxOffice.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SalePdfDocumentTypeVM {
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => x.PaymentMethod.Description))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new SalePdfAadeVM {
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    QrUrl = x.Aade.QrUrl
                }))
                .ForMember(x => x.Summary, x => x.MapFrom(x => new SalePdfSummaryVM {
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.VatAmount,
                    GrossAmount = x.GrossAmount
                }));
            CreateMap<Sale, SaleBalanceVM>()
                .ForMember(x => x.PreviousBalance, x => x.MapFrom(x => x.PreviousBalance))
                .ForMember(x => x.NewBalance, x => x.MapFrom(x => x.NewBalance));
        }

    }

}