using API.Infrastructure.EmailServices;

namespace API.Features.Reservations.Transactions {

    public interface IEndOfLeaseEmailSender {

        Task SendEndOfLeaseToEmail(EmailQueue emailQueue, Reservation reservation);

    }

}