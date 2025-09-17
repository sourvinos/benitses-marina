using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Common.PaymentMethods {

    public interface IPaymentMethodRepository : IRepository<PaymentMethod> {

        Task<IEnumerable<PaymentMethodListVM>> GetAsync();
        Task<IEnumerable<PaymentMethodBrowserVM>> GetForBrowserAsync();
        Task<PaymentMethodBrowserVM> GetByIdForBrowserAsync(int id);
        Task<PaymentMethod> GetByIdAsync(int id, bool includeTables);

    }

}