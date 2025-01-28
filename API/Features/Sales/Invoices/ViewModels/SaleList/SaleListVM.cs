using API.Infrastructure.Classes;

namespace API.Features.Sales.Invoices {

    public class InvoiceistVM {

        public string InvoiceId { get; set; }
        public string Date { get; set; }
        public int InvoiceNo { get; set; }
        public SimpleEntity Customer { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public decimal GrossAmount { get; set; }
        public bool IsEmailPending { get; set; }
        public bool IsEmailSent { get; set; }
        public SaleListAadeVM Aade { get; set; }

    }

}