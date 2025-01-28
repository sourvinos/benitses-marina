namespace API.Features.Sales.Invoices {

    public class DataUpJsonInvoiceVM {

        public string Issue_date { get; set; }
        public string Series { get; set; }
        public decimal Gross_price { get; set; }
        public string Payment_type { get; set; }
        public string Branch { get; set; }
        public string Issuer_vat_number { get; set; }
        public string Mydata_transmit { get; set; }

    }

}