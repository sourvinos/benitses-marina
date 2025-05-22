using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonInvoiceVM {

        [JsonProperty("issue_date")]
        public string Issue_date { get; set; }

        [JsonProperty("series")]
        public string Series { get; set; }

        [JsonProperty("gross_price")]
        public decimal Gross_price { get; set; }

        [JsonProperty("payment_type")]
        public string Payment_type { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("issuer_vat_number")]
        public string Issuer_vat_number { get; set; }

        [JsonProperty("mydata_transmit")]
        public string Mydata_transmit { get; set; }

    }

}