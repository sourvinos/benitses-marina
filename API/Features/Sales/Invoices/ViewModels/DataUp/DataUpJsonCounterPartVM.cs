namespace API.Features.Sales.Invoices {

    public class DataUpJsonCounterPartVM {

        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Vat_number { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
        public DataUpJsonAddressVM Address { get; set; }

    }

}