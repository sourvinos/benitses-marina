using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.Features.Expenses.Companies;
using API.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public class InvoiceDataUpRepository : IInvoiceDataUpRepository {

        public DataUpJsonVM CreateJsonInvoice(Company company, Invoice invoice) {
            var x = new DataUpJsonVM {
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
                    Vat_Number = invoice.Customer.VatNumber,
                    Tax_Code = invoice.Customer.VatPercentId,
                    Tax_Exception = invoice.Customer.VatExemptionId,
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
            return x;
        }

        public string SaveJsonInvoice(DataUpJsonVM x) {
            var jsonString = JsonConvert.SerializeObject(x);
            var fullPathname = FileSystemHelpers.CreateInvoiceJsonFullPathName(x, "Jsons", "invoice");
            using StreamWriter outputFile = new(fullPathname);
            outputFile.Write(jsonString);
            return jsonString;
        }

        public async Task<string> UploadJsonInvoiceAsync(string x, Company company) {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", company.DemoToken);
            var content = new StringContent(x, UTF8Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://dataup-uat.gr/api/docking/invoice/add", content);
            return await response.Content.ReadAsStringAsync();
        }

        public JObject ShowResponseAfterUploadJsonInvoice(string response) {
            return JObject.Parse(response);
        }

        private static List<DataUpJsonLineVM> AddLineItems(List<InvoiceItem> lineItems) {
            var x = new List<DataUpJsonLineVM>();
            foreach (var lineItem in lineItems) {
                var z = new DataUpJsonLineVM() {
                    Description = lineItem.EnglishDescription,
                    Gross_price = lineItem.GrossAmount,
                    Net_price = lineItem.NetAmount,
                    Quantity = lineItem.Quantity,
                    Tax_code = lineItem.TaxCode,
                    Title = lineItem.Description
                };
                x.Add(z);
            }
            return x;
        }

    }

}