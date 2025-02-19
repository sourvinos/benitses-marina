using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonVM {

        [JsonPropertyName("contract")]
        public string Contract { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("vessel_name")]
        public string Vessel_name { get; set; }

        [JsonPropertyName("invoice")]
        public DataUpJsonInvoiceVM Invoice { get; set; }

        [JsonPropertyName("counterpart")]
        public DataUpJsonCounterPartVM CounterPart { get; set; }

        [JsonPropertyName("lines")]
        public IEnumerable<DataUpJsonLineVM> Lines { get; set; }

    }

}