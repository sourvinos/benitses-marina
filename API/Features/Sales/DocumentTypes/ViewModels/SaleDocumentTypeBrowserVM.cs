namespace API.Features.Sales.DocumentTypes {

    public class SaleDocumentTypeBrowserVM {

        public int Id { get; set; }
        public int DiscriminatorId { get; set; }
        public string AbbreviationEn { get; set; }
        public string Description { get; set; }
        public string Batch { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

    }

}