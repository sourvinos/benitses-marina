using API.Infrastructure.EmailServices;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Features.Reservations.Transactions {

    public class InvalidInsuranceEmailSender : IInvalidInsuranceEmailSender {

        #region variables

        private readonly EmailReservationSettings emailReservationSettings;

        #endregion

        public InvalidInsuranceEmailSender(IOptions<EmailReservationSettings> emailReservationSettings) {
            this.emailReservationSettings = emailReservationSettings.Value;
        }

        public async Task SendInvalidInsuranceToEmail(EmailQueue emailQueue, string boat, string email) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailReservationSettings.SmtpClient, emailReservationSettings.Port);
            smtp.Authenticate(emailReservationSettings.Username, emailReservationSettings.Password);
            await smtp.SendAsync(await BuildMessage(emailQueue, boat, email));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildMessage(EmailQueue model, string boat, string email) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailReservationSettings.Username) };
            message.From.Add(new MailboxAddress(emailReservationSettings.From, emailReservationSettings.Username));
            message.To.AddRange(BuildReceivers(email));
            message.Subject = "⚓ Urgent: Invalid insurance for vessel '" + boat + "'";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailReservationTemplateAsync(model) };
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static InternetAddressList BuildReceivers(string email) {
            InternetAddressList internetAddressList = new();
            var emails = email.Split(",");
            foreach (string address in emails) {
                internetAddressList.Add(MailboxAddress.Parse(EmailHelpers.BeValidEmailAddress(address.Trim()) ? address.Trim() : "postmaster@appbenitsesmarina.com"));
            }
            return internetAddressList;
        }

        private static async Task<string> BuildEmailReservationTemplateAsync(EmailQueue model) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync("key", LoadTemplateFromFile(), new EmailInvalidInsuranceVM {
                UserFullname = model.UserFullname,
                Email = "info@benitsesmarina.com",
                CompanyPhones = "+30 26610 72627, +30 6937 133 662",
                Website = "www.benitsesmarina.com"
            });
        }

        private static string LoadTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailInvalidInsurance.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

    }

}