using AutoMapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    public class InvoiceEmailScheduleSender : ISaleEmailScheduleSender, IHostedService, IDisposable {

        private Timer timer;
        private readonly ISaleReadRepository invoiceReadRepo;
        private readonly ISaleEmailSender invoiceEmailSender;
        private readonly ISalePdfRepository invoicePdfRepo;
        private readonly IMapper mapper;

        public InvoiceEmailScheduleSender(ISaleEmailSender invoiceEmailSender, ISalePdfRepository invoicePdfRepo, ISaleReadRepository invoiceReadRepo, IMapper mapper) {
            this.invoicePdfRepo = invoicePdfRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceEmailSender = invoiceEmailSender;
            this.mapper = mapper;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            timer = new Timer(SendInvoicesToEmailScheduleAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public void SendInvoicesToEmailScheduleAsync(object state) {
            var x = invoiceReadRepo.GetFirstWithEmailPending();
            if (x != null) {
                var z = invoicePdfRepo.BuildPdf(x);
                if (z != "") {
                    string[] dessert = new string[] { z };
                    var i = new EmailSaleVM {
                        CustomerId = 2,
                        Filenames = dessert
                    };
                    var response = invoiceEmailSender.SendInvoicesToEmail(i);
                    Console.WriteLine(response);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() {
            timer?.Dispose();
        }

    }

}