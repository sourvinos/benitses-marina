using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Customers {

    public interface ICustomerValidation : IRepository<Customer> {

        Task<int> IsValidAsync(Customer x, CustomerWriteDto customer);
        Task<int> IsValidWithWarningAsync(CustomerWriteDto customer);

    }

}