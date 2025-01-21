using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    public interface ISaleValidation {

        Task<int> IsValidAsync(Sale x, SaleWriteDto invoice);

    }

}