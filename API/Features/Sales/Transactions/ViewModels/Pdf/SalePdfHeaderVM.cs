namespace API.Features.Sales.Transactions {

    public class SalePdfHeaderVM {

        public string Date { get; set; }
        public string TripDate { get; set; }
        public SalePdfDocumentTypeVM DocumentType { get; set; }
        public int InvoiceNo { get; set; }

    }

}