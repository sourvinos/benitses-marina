namespace API.Features.Sales.Invoices {

    public class DataUpJsonLineVM {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tax_code { get; set; }
        public int Quantity { get; set; }
        public decimal Net_price { get; set; }
        public decimal Gross_price { get; set; }

    }

}