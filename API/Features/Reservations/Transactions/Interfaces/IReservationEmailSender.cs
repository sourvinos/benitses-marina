using API.Infrastructure.EmailServices;

namespace API.Features.Reservations.Transactions {

    public interface IReservationEmailSender {

        Task SendReservationToEmail(EmailQueue emailQueue, string boat, string email);

    }

}