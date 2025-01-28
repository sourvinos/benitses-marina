using System.Collections.Generic;

namespace API.Features.Sales.Invoices {

    public class DataUpJsonVM {

        public string Contract { get; set; }
        public string Position { get; set; }
        public string Vessel_name { get; set; }
        public DataUpJsonInvoiceVM Invoice { get; set; }
        public DataUpJsonCounterPartVM CounterPart { get; set; }
        public IEnumerable<DataUpJsonLineVM> LineItems { get; set; }

    }

}