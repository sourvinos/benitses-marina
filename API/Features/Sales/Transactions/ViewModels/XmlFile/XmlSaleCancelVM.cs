namespace API.Features.Sales.Transactions {

    public class XmlSaleCancelVM {

        public XmlCredentialsVM Credentials { get; set; }
        public XmlSaleHeaderVM InvoiceHeader { get; set; }
        public string Mark { get; set; }

    }

}