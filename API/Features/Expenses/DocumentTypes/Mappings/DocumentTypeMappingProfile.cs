using AutoMapper;

namespace API.Features.Expenses.DocumentTypes {

    public class DocumentTypeMappingProfile : Profile {

        public DocumentTypeMappingProfile() {
            CreateMap<DocumentType, DocumentTypeListVM>();
            CreateMap<DocumentType, DocumentTypeBrowserVM>();
            CreateMap<DocumentType, DocumentTypeReadDto>();
            CreateMap<DocumentTypeWriteDto, DocumentType>();
        }

    }

}