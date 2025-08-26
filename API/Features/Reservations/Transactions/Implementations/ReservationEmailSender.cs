using API.Features.Reservations.Parameters;
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

    public class ReservationEmailSender : IReservationEmailSender {

        #region variables

        private readonly EmailReservationSettings emailReservationSettings;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReservationEmailSender(IOptions<EmailReservationSettings> emailReservationSettings, IReservationParametersRepository parametersRepo) {
            this.emailReservationSettings = emailReservationSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task SendReservationToEmail(EmailQueue emailQueue, string email) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailReservationSettings.SmtpClient, emailReservationSettings.Port);
            smtp.Authenticate(emailReservationSettings.Username, emailReservationSettings.Password);
            await smtp.SendAsync(await BuildReservationMessage(emailQueue, email));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildReservationMessage(EmailQueue model, string email) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailReservationSettings.Username) };
            message.From.Add(new MailboxAddress(emailReservationSettings.From, emailReservationSettings.Username));
            message.To.AddRange(BuildReceivers(email));
            message.Subject = "✨ Αποστολή παραστατικών παροχής υπηρεσιών";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailReservationTemplate() };
            var filenames = model.Filenames.Split(",");
            foreach (var filename in filenames) {
                builder.Attachments.Add(Path.Combine("Uploaded Lease Agreements" + Path.DirectorySeparatorChar + model.EntityId.ToString() + " " + filename + ".pdf"));
            }
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static InternetAddressList BuildReceivers(string email) {
            InternetAddressList internetAddressList = new();
            var emails = email.Split(",");
            foreach (string address in emails) {
                internetAddressList.Add(MailboxAddress.Parse(EmailHelpers.BeValidEmailAddress(address.Trim()) ? address.Trim() : "postmaster@appcorfucruises.com"));
            }
            return internetAddressList;
        }

        private async Task<string> BuildEmailReservationTemplate() {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync("key", LoadEmailReservationTemplateFromFile(), new EmailReservationTemplateVM {
                Email = "parametersRepo.GetAsync().Result.Email",
                CompanyPhones = "parametersRepo.GetAsync().Result.Phones",
                Website = "parametersRepo.GetAsync().Result.Website"
            });
        }

        private static string LoadEmailReservationTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailReservation.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

    }

}