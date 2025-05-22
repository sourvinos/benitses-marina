using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonLineVM {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("tax_code")]
        public string Tax_code { get; set; }

        [JsonProperty("tax_exception")]
        public string Tax_Exception { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("net_price")]
        public decimal Net_price { get; set; }

        [JsonProperty("gross_price")]
        public decimal Gross_price { get; set; }

    }

}