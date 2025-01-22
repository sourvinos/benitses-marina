using API.Infrastructure.Interfaces;

namespace API.Features.Sales.DocumentTypes {

    public interface ISaleDocumentTypeValidation : IRepository<SaleDocumentType> {

        int IsValid(SaleDocumentType x, SaleDocumentTypeWriteDto documentType);

    }

}