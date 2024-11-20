using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Interfaces;

namespace API.Features.InsurancePolicies {

    public interface IInsurancePolicyRepository : IRepository<ReservationInsurance> {

        Task<IEnumerable<InsurancePolicyListVM>> GetAsync();

    }

}