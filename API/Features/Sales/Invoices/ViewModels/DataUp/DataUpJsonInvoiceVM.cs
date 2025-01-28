using System.Text.Json.Serialization;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonInvoiceVM {

        [JsonPropertyName("issue_date")]
        public string Issue_date { get; set; }

        [JsonPropertyName("series")]
        public string Series { get; set; }

        [JsonPropertyName("gross_price")]
        public decimal Gross_price { get; set; }

        [JsonPropertyName("payment_type")]
        public string Payment_type { get; set; }

        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        [JsonPropertyName("issuer_vat_number")]
        public string Issuer_vat_number { get; set; }

        [JsonPropertyName("mydata_transmit")]
        public string Mydata_transmit { get; set; }

    }

}