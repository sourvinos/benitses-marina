using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Reservations.Transactions {

    public interface IInvalidInsuranceEmailSender {

        Task SendInvalidInsuranceToEmail(EmailQueue emailQueue, string boat, string email);

    }

}