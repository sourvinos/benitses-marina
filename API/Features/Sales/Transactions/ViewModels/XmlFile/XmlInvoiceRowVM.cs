namespace API.Features.Sales.Transactions {

    public class XmlInvoiceRowVM {

        public int LineNumber { get; set; }
        public decimal NetValue { get; set; }
        public int VatCategory { get; set; }
        public decimal VatAmount { get; set; }
        public int VatExemptionCategory { get; set; }

    }

}