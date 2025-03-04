namespace API.Features.Sales.Invoices {

    public class InvoiceItemReadDto {

        public int Id { get; set; }
        public string InvoiceId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public string Remarks { get; set; }
        public int Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }

}