using System.Text.Json.Serialization;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonCounterPartVM {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }

        [JsonPropertyName("vat_number")]
        public string Vat_Number { get; set; }

        [JsonPropertyName("tax_code")]
        public int Tax_Code { get; set; }

        [JsonPropertyName("tax_exception")]
        public int Tax_Exception { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        public DataUpJsonAddressVM Address { get; set; }

    }

}