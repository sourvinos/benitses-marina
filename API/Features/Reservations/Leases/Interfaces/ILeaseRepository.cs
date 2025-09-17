using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Transactions;
using API.Infrastructure.Interfaces;

namespace API.Features.Leases {

    public interface ILeaseRepository : IRepository<Reservation> {

        Task<IEnumerable<LeaseUpcomingTerminationListVM>> GetAsync(int days);

    }

}