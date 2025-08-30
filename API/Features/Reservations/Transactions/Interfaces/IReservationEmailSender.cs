using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Reservations.Transactions {

    public interface IReservationEmailSender {

        Task SendReservationToEmail(EmailQueue emailQueue);

    }

}