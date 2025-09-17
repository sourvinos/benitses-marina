using System.Linq;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class SaleCreateMappingProfile : Profile {

        public SaleCreateMappingProfile() {
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 1))
                .ForMember(x => x.NetAmount, x => x.MapFrom(x => x.Items.Sum(x => x.NetAmount)))
                .ForMember(x => x.VatAmount, x => x.MapFrom(x => x.Items.Sum(x => x.NetAmount * (x.VatPercent / 100))))
                .ForMember(x => x.GrossAmount, x => x.MapFrom(x => x.Items.Sum(x => x.NetAmount + x.VatAmount)))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks == null ? "" : x.Remarks.Trim()))
                .ForMember(x => x.Items, x => x.MapFrom(x => x.Items.Select(x => new InvoiceItemWriteDto {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    Code = x.Code,
                    Description = x.Description,
                    EnglishDescription = x.EnglishDescription,
                    TaxCode = x.TaxCode,
                    TaxException = x.TaxException,
                    Quantity = x.Quantity,
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.NetAmount * (x.VatPercent / 100),
                    GrossAmount = x.NetAmount + x.VatAmount,
                    Remarks = x.Remarks ?? "",
                })));
            CreateMap<InvoiceItemWriteDto, InvoiceItem>();
        }

    }

}