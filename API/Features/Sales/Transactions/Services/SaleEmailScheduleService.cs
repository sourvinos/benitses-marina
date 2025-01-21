using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Features.Sales.Transactions {

    public class SaleEmailScheduleService : BackgroundService {

        private readonly ISaleEmailSender saleEmailSender;
        private readonly ISalePdfRepository salePdfRepo;
        private readonly ISaleReadRepository saleReadRepo;
        private readonly IServiceProvider serviceProvider;

        public SaleEmailScheduleService(ISaleEmailSender invoiceEmailSender, ISalePdfRepository invoicePdfRepo, ISaleReadRepository invoiceReadRepo, IServiceProvider serviceProvider) {
            this.saleEmailSender = invoiceEmailSender;
            this.salePdfRepo = invoicePdfRepo;
            this.saleReadRepo = invoiceReadRepo;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
                var x = saleReadRepo.GetFirstWithEmailPending();
                if (x != null) {
                    // await saleEmailSender.SendSalesToEmail(BuildVM(x));
                    await PatchSaleEmailFields(x);
                }
            }
        }

        private EmailSaleVM BuildVM(SalePdfVM x) {
            string[] filenames = { salePdfRepo.BuildPdf(x) };
            return new EmailSaleVM {
                CustomerId = x.Customer.Id,
                Filenames = filenames
            };
        }

        private async Task PatchSaleEmailFields(SalePdfVM invoiceVM) {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var invoice = await saleReadRepo.GetByIdForPatchEmailSent(invoiceVM.InvoiceId.ToString());
            invoice.IsEmailPending = false;
            invoice.IsEmailSent = true;
            dbContext.Sales.Attach(invoice);
            dbContext.Entry(invoice).Property(x => x.IsEmailPending).IsModified = true;
            dbContext.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            dbContext.SaveChanges();
        }

    }

}