using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.PaymentStatuses {

    public interface IPaymentStatusRepository : IRepository<PaymentStatus> {

        Task<IEnumerable<PaymentStatusListVM>> GetAsync();
        Task<IEnumerable<PaymentStatusBrowserVM>> GetForBrowserAsync();
        Task<PaymentStatusBrowserVM> GetByIdForBrowserAsync(int id);
        Task<PaymentStatus> GetByIdAsync(int id, bool includeTables);

    }

}