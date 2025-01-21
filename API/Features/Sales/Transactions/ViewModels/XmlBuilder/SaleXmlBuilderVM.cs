using System;

namespace API.Features.Sales.Transactions {

    public class SaleXmlBuilderVM {

        public Guid InvoiceId { get; set; }
        public XmlCredentialsVM Credentials { get; set; }
        public XmlPartyVM Issuer { get; set; }
        public XmlPartyVM CounterPart { get; set; }
        public XmlSaleHeaderVM InvoiceHeader { get; set; }
        public XmlPaymentMethodVM PaymentMethod { get; set; }
        public XmlInvoiceRowVM InvoiceDetail { get; set; }
        public XmlInvoiceSummaryVM InvoiceSummary { get; set; }
        public XmlAadeVM Aade { get; set; }

    }

}