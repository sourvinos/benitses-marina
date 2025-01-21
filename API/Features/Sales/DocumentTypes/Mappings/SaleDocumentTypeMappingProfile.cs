using AutoMapper;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeMappingProfile : Profile {

        public DocumentTypeMappingProfile() {
            // List
            CreateMap<SaleDocumentType, SaleDocumentTypeListVM>();
            // Browser
            CreateMap<SaleDocumentType, SaleDocumentTypeBrowserVM>()
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.AbbreviationEn))
                .ForMember(x => x.Batch, x => x.MapFrom(x => x.BatchEn));
            // Read
            CreateMap<SaleDocumentType, DocumentTypeReadDto>();
            // Write
            CreateMap<SaleDocumentTypeWriteDto, SaleDocumentType>()
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Batch, x => x.MapFrom(x => x.Batch.Trim()))
                .ForMember(x => x.BatchEn, x => x.MapFrom(x => x.BatchEn.Trim()));
        }

    }

}