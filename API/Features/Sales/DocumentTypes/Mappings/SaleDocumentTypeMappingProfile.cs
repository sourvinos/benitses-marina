using AutoMapper;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeMappingProfile : Profile {

        public DocumentTypeMappingProfile() {
            // List
            CreateMap<SaleDocumentType, SaleDocumentTypeListVM>();
            // Browser
            CreateMap<SaleDocumentType, SaleDocumentTypeBrowserVM>();
            // Read
            CreateMap<SaleDocumentType, DocumentTypeReadDto>();
            // Write
            CreateMap<SaleDocumentTypeWriteDto, SaleDocumentType>()
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()))
                .ForMember(x => x.AbbreviationEn, x => x.MapFrom(x => x.AbbreviationEn.Trim()))
                .ForMember(x => x.AbbreviationDataUp, x => x.MapFrom(x => x.AbbreviationDataUp.Trim()))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Batch, x => x.MapFrom(x => x.Batch.Trim()));
        }

    }

}