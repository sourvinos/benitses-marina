using System.Text.Json.Serialization;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonLineVM {

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("tax_code")]
        public string Tax_code { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("net_price")]
        public decimal Net_price { get; set; }

        [JsonPropertyName("gross_price")]
        public decimal Gross_price { get; set; }

    }

}