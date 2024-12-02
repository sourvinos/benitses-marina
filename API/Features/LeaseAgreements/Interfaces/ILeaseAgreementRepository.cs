using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.LeaseAgreements {

    public interface ILeaseAgreementRepository : IRepository<Reservation> {

        Task<LeaseAgreementVM> GetByIdAsync(string reservationId);
        string BuildLeaseAgreement(LeaseAgreementVM leaseAgreement);
        FileStreamResult OpenPdf(string filename);

    }

}