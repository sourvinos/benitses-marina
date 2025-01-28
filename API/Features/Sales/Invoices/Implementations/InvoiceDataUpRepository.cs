using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public class InvoiceDataUpRepository : IInvoiceDataUpRepository {

        public DataUpJsonVM CreateJsonFileAsync(Invoice invoice) {
            DataUpJsonVM json = new() {
                Contract = invoice.InvoiceId.ToString(),
                Position = "position",
                Vessel_name = "vessel_name",
                Invoice = new() {
                    Issue_date = DateHelpers.DateToISOString(invoice.Date),
                    Series = "series",
                    Gross_price = invoice.GrossAmount,
                    Payment_type = invoice.PaymentMethod.MyDataId.ToString(),
                    Branch = "branch",
                    Issuer_vat_number = "issuer_vat_number",
                    Mydata_transmit = "true"
                },
                CounterPart = new() {
                    Name = invoice.Customer.Description,
                    Firstname = "first_name",
                    Lastname = "last_name",
                    Vat_number = invoice.Customer.VatNumber,
                    Country = invoice.Customer.Nationality.Code,
                    Branch = invoice.Customer.Branch.ToString(),
                    Address = new() {
                        PostCode = invoice.Customer.PostalCode,
                        City = invoice.Customer.City,
                        Number = invoice.Customer.Number,
                        Street = invoice.Customer.Street
                    }
                },
                LineItems = AddLineItems(invoice.LineItems)
            };
            return json;
        }

        public async Task<JObject> UploadJsonAsync() {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dataup-uat.gr/api/docking/invoice/add");
            request.Headers.Add("Authorization", "Bearer 3a78c28fda85b253c8446c5fbe664b54da71775c112c85f22cc1995e986620a2");
            var content = new StringContent("{\n    \"contract\": \"guid-18\",\n    \"vessel_name\": \"ANEMOS\",\n    \"invoice\": {\n        \"issue_date\": \"2025-01-28\",\n        \"series\": \"ΑΠΥ\",\n        \"gross_price\": 134,\n        \"payment_type\": \"5\",\n        \"branch\": \"0\",\n        \"issuer_vat_number\": \"801515394\",\n        \"mydata_transmit\": \"true\"\n    },\n    \"counterpart\": {\n        \"name\": \"Test Digital Valley\",\n        \"vat_number\": \"800812329\",\n        \"country\": \"GR\",\n        \"branch\": \"0\",\n        \"address\": {\n            \"postal_code\": \"11111\",\n            \"city\": \"athens\",\n            \"number\": \"10\",\n            \"street\": \"stadiou\"\n        }\n    },\n    \"lines\": [\n        {\n            \"title\": \"Daily mooring 15m\",\n            \"description\": \"\",\n            \"tax_code\": \"1\",\n            \"quantity\": 1,\n            \"net_price\": 100,\n            \"gross_price\": 124\n        },\n        {\n            \"title\": \"Pillar Card\",\n            \"description\": \"\",\n            \"tax_code\": \"1\",\n            \"quantity\": 1,\n            \"net_price\": 8.06,\n            \"gross_price\": 10\n        }\n    ]\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            var x = JObject.Parse(await response.Content.ReadAsStringAsync());
            return x;
        }

        private static List<DataUpJsonLineVM> AddLineItems(List<InvoiceItem> lineItems) {
            var x = new List<DataUpJsonLineVM>();
            foreach (var lineItem in lineItems) {
                var z = new DataUpJsonLineVM() {
                    Title = "title",
                    Description = "",
                    Tax_code = "1",
                    Gross_price = lineItem.GrossAmount
                };
                x.Add(z);
            }
            return x;
        }

    }

}