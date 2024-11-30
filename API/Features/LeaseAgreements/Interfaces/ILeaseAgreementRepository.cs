using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Interfaces;

namespace API.Features.LeaseAgreements {

    public interface ILeaseAgreementRepository : IRepository<Reservation> {

        Task<LeaseAgreementVM> GetByIdAsync(string reservationId);
        void BuildLeaseAgreement(LeaseAgreementVM leaseAgreement);

    }

}