using API.Features.Sales.Customers;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    public class InvoiceEmailSender : ISaleEmailSender {

        #region variables

        private readonly EmailInvoicingSettings emailSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IMapper mapper;

        #endregion

        public InvoiceEmailSender(ICustomerRepository customerRepo, IOptions<EmailInvoicingSettings> emailSettings, IMapper mapper) {
            this.customerRepo = customerRepo;
            this.emailSettings = emailSettings.Value;
            this.mapper = mapper;
        }

        #region public methods

        public async Task SendInvoicesToEmail(EmailSaleVM model) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.SmtpClient, emailSettings.Port, false);
            smtp.Authenticate(emailSettings.Username, emailSettings.Password);
            await smtp.SendAsync(await BuildInvoiceMessage(model));
            smtp.Disconnect(true);
        }

        #endregion

        #region private methods

        private async Task<MimeMessage> BuildInvoiceMessage(EmailSaleVM model) {
            var customer = GetCustomerAsync(model.CustomerId).Result;
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailSettings.Username) };
            message.From.Add(new MailboxAddress(emailSettings.From, emailSettings.Username));
            message.To.AddRange(BuildReceivers(customer.Email));
            message.Subject = "📧 Ηλεκτρονική αποστολή παραστατικών";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailInvoiceTemplate(customer.Email) };
            foreach (var filename in model.Filenames) {
                builder.Attachments.Add(Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename));
            }
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static InternetAddressList BuildReceivers(string email) {
            InternetAddressList x = new();
            var emails = email.Split(",");
            foreach (string address in emails) {
                x.Add(MailboxAddress.Parse(EmailHelpers.BeValidEmailAddress(address.Trim()) ? address.Trim() : "postmaster@appcorfucruises.com"));
            }
            return x;
        }

        private async Task<string> BuildEmailInvoiceTemplate(string email) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadEmailInvoiceTemplateFromFile(),
                new EmailSaleTemplateVM {
                    Email = email,
                    CompanyPhones = "",
                });
        }

        private static string LoadEmailInvoiceTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailInvoice.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private async Task<EmailSaleCustomerVM> GetCustomerAsync(int id) {
            var x = await customerRepo.GetByIdAsync(id, false);
            if (x != null) {
                return mapper.Map<Customer, EmailSaleCustomerVM>(x);
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        #endregion

    }

}