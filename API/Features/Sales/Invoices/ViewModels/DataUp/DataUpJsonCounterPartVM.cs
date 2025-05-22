using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonCounterPartVM {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("vat_number")]
        public string Vat_Number { get; set; }

        [JsonProperty("tax_code")]
        public int Tax_Code { get; set; }

        [JsonProperty("tax_exception")]
        public int Tax_Exception { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        public DataUpJsonAddressVM Address { get; set; }

    }

}