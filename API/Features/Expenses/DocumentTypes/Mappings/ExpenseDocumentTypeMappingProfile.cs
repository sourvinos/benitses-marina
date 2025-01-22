using AutoMapper;

namespace API.Features.Expenses.DocumentTypes {

    public class ExpenseDocumentTypeMappingProfile : Profile {

        public ExpenseDocumentTypeMappingProfile() {
            CreateMap<ExpenseDocumentType, ExpenseDocumentTypeListVM>();
            CreateMap<ExpenseDocumentType, ExpenseDocumentTypeBrowserVM>();
            CreateMap<ExpenseDocumentType, ExpenseDocumentTypeReadDto>();
            CreateMap<ExpenseDocumentTypeWriteDto, ExpenseDocumentType>();
        }

    }

}