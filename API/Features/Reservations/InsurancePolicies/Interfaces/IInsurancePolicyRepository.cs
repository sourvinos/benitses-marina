using API.Features.Reservations.Transactions;
using API.Infrastructure.Interfaces;

namespace API.Features.InsurancePolicies {

    public interface IInsurancePolicyRepository : IRepository<ReservationInsurance> {

        Task<IEnumerable<InsurancePolicyListVM>> GetAsync();

    }

}