using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Suppliers {

    public interface ISupplierValidation : IRepository<Supplier> {

        int IsValid(Supplier x, SupplierWriteDto Supplier);
        Task<int> IsValidWithWarningAsync(SupplierWriteDto Supplier);

    }

}