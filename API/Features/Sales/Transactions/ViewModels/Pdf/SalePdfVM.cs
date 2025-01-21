using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Sales.Transactions {

    public class SalePdfVM {

        public Guid InvoiceId { get; set; }
        public SalePdfHeaderVM Header { get; set; }
        public string Remarks { get; set; }
        public SalePdfShipVM Ship { get; set; }
        public SalePdfAadeVM Aade { get; set; }
        public List<SalePdfPortVM> Ports { get; set; }
        public SalePdfPartyVM Customer { get; set; }
        public string Destination { get; set; }
        public SalePdfDocumentTypeVM DocumentType { get; set; }
        public string PaymentMethod { get; set; }
        public SalePdfPartyVM Issuer { get; set; }
        public SalePdfSummaryVM Summary { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
        public SimpleEntity[] BankAccounts { get; set; }

    }

}