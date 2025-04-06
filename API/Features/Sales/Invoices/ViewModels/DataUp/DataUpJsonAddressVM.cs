using System.Text.Json.Serialization;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonAddressVM {

        [JsonPropertyName("postal_code")]
        public string Postal_Code { get; set; }
        
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

    }

}