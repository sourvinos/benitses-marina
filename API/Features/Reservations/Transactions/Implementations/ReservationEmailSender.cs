using API.Infrastructure.EmailServices;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using API.Infrastructure.Users;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly IReservationRepository reservationRepo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public ReservationEmailSender(IHttpContextAccessor httpContextAccessor, IReservationRepository reservationRepo, IOptions<EmailReservationSettings> emailReservationSettings, UserManager<UserExtended> userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.emailReservationSettings = emailReservationSettings.Value;
            this.reservationRepo = reservationRepo;
            this.userManager = userManager;
        }

        public async Task SendReservationToEmail(EmailQueue emailQueue) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailReservationSettings.SmtpClient, emailReservationSettings.Port);
            smtp.Authenticate(emailReservationSettings.Username, emailReservationSettings.Password);
            await smtp.SendAsync(await BuildReservationMessage(emailQueue));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildReservationMessage(EmailQueue model) {
            var emailRecipients = await GetReceipientEmails(model.EntityId.ToString());
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailReservationSettings.Username) };
            message.From.Add(new MailboxAddress(emailReservationSettings.From, emailReservationSettings.Username));
            message.To.AddRange(BuildReceivers(emailRecipients));
            message.Subject = "⚓ Invoice & lease agreement for vessel '***'";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailReservationTemplateAsync(model) };
            var filenames = model.Filenames.Split(",");
            foreach (var filename in filenames) {
                builder.Attachments.Add(Path.Combine("Uploaded Lease Agreements" + Path.DirectorySeparatorChar + model.EntityId.ToString() + " " + filename));
            }
            builder.Attachments.Add(Path.Combine("Documents" + Path.DirectorySeparatorChar + "Pillar instructions.pdf"));
            builder.Attachments.Add(Path.Combine("Documents" + Path.DirectorySeparatorChar + "Wi-Fi and Showers Passkeys.pdf"));
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
            return await engine.CompileRenderStringAsync("key", LoadEmailReservationTemplateFromFile(), new EmailReservationTemplateVM {
                UserDisplayname = model.UserDisplayName,
                Email = "info@benitsesmarina.com",
                CompanyPhones = "+30 26610 72150",
                Website = "www.benitsesmarina.com"
            });
        }

        private static string LoadEmailReservationTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailReservation.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private async Task<string> GetReceipientEmails(string reservationId) {
            var x = await reservationRepo.GetByIdAsync(reservationId, false);
            if (x != null) {
                return x.Owner.Email;
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}