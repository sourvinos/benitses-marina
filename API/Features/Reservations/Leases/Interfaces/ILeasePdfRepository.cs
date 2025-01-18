using System.Threading.Tasks;
using API.Features.Reservations.Transactions;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Leases {

    public interface ILeasePdfRepository : IRepository<Reservation> {

        Task<LeasePdfVM> GetByIdAsync(string reservationId);
        string BuildLeasePdf(LeasePdfVM leaseAgreement);
        FileStreamResult OpenPdf(string filename);

    }

}