using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Expenses.Companies;
using API.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public class InvoiceDataUpRepository : IInvoiceDataUpRepository {

        public DataUpJsonVM CreateJsonFileAsync(Company company, Invoice invoice) {
            DataUpJsonVM json = new() {
                Contract = invoice.InvoiceId.ToString(),
                Position = "position",
                Vessel_name = "vessel_name",
                Invoice = new() {
                    Issue_date = DateHelpers.DateToISOString(invoice.Date),
                    Series = invoice.DocumentType.AbbreviationDataUp,
                    Gross_price = invoice.GrossAmount,
                    Payment_type = invoice.PaymentMethod.MyDataId.ToString(),
                    Branch = "0",
                    Issuer_vat_number = company.TaxNo,
                    Mydata_transmit = "true"
                },
                CounterPart = new() {
                    Name = invoice.Customer.Description,
                    Firstname = "",
                    Lastname = "",
                    Vat_number = invoice.Customer.VatNumber,
                    Country = invoice.Customer.Nationality.Code,
                    Branch = invoice.Customer.Branch.ToString(),
                    Address = new() {
                        Postal_Code = invoice.Customer.PostalCode,
                        City = invoice.Customer.City,
                        Number = invoice.Customer.Number,
                        Street = invoice.Customer.Street
                    }
                },
                Lines = AddLineItems(invoice.Items)
            };
            return json;
        }

        public async Task<JObject> UploadJsonAsync(Company company, DataUpJsonVM json) {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://dataup-uat.gr/api/docking/invoice/add");
            var content = new StringContent(JsonSerializer.Serialize(json), System.Text.Encoding.UTF8, "application/json");
            var token = company.IsDemoMyData ? company.DemoToken : company.LiveToken;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = content;
            var response = await client.SendAsync(request);
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        private static List<DataUpJsonLineVM> AddLineItems(List<InvoiceItem> lineItems) {
            var x = new List<DataUpJsonLineVM>();
            foreach (var lineItem in lineItems) {
                var z = new DataUpJsonLineVM() {
                    Title = lineItem.Description,
                    Description = lineItem.EnglishDescription,
                    Tax_code = "1",
                    Quantity = lineItem.Quantity,
                    Net_price = lineItem.NetAmount,
                    Gross_price = lineItem.GrossAmount
                };
                x.Add(z);
            }
            return x;
        }

    }

}