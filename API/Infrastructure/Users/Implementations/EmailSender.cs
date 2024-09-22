using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    public class EmailSender : IEmailSender {

        private readonly EmailSettings emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings) {
            this.emailSettings = emailSettings.Value;
        }

        public async Task EmailUserDetails(UserDetailsForEmailVM model) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.SmtpClient, emailSettings.BoatType);
            smtp.Authenticate(emailSettings.Username, emailSettings.Password);
            await smtp.SendAsync(await BuildMessage(model));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildMessage(UserDetailsForEmailVM model) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailSettings.Username) };
            message.From.Add(new MailboxAddress(emailSettings.From, emailSettings.Username));
            message.To.Add(MailboxAddress.Parse(model.Email));
            message.Subject = model.Subject;
            message.Body = new BodyBuilder { HtmlBody = await BuildTemplate(model) }.ToMessageBody();
            return message;
        }

        private async Task<string> BuildTemplate(UserDetailsForEmailVM model) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadNewUserEmailTemplateFromFile(),
                new UserDetailsForEmailVM {
                    Username = model.Username,
                    Displayname = model.Displayname,
                    Email = model.Email,
                    Url = model.Url,
                    CompanyPhones = "",
                    LogoTextBase64 = SetLogoTextAsBackground()
                });
        }

        private static string LoadNewUserEmailTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UserDetailsForEmail.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private static string SetLogoTextAsBackground() {
            return "width: 116px; height: 23px; background: url(data:image/png;base64," + LogoService.GetBase64LogoText() + ")";
        }

    }

}