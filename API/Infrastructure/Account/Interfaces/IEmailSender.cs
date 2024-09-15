using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    public interface IEmailSender {

        Task SendForgotPasswordEmail(string username, string displayname, string email, string callbackUrl, string subject);

    }

}