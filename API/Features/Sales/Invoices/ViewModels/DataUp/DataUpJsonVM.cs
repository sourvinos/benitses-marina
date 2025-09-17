using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonVM {

        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("vessel_name")]
        public string Vessel_name { get; set; }

        [JsonProperty("invoice")]
        public DataUpJsonInvoiceVM Invoice { get; set; }

        [JsonProperty("counterpart")]
        public DataUpJsonCounterPartVM CounterPart { get; set; }

        [JsonProperty("lines")]
        public IEnumerable<DataUpJsonLineVM> Lines { get; set; }

    }

}