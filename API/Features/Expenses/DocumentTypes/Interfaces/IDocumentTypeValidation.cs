using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public interface IDocumentTypeValidation : IRepository<DocumentType> {

        int IsValid(DocumentType x, DocumentTypeWriteDto documentType);

    }

}