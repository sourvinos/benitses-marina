namespace API.Features.Sales.Invoices {

    public class InvoiceCustomerVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsActive { get; set; }

    }

}