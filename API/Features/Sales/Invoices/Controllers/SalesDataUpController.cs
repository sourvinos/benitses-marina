using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class SalesDataUpController : ControllerBase {

        #region variables

        private readonly IInvoiceDataUpRepository dataUpRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        // private readonly HttpClient httpClient;

        #endregion

        public SalesDataUpController(IInvoiceReadRepository invoiceReadRepo, IInvoiceDataUpRepository dataUpRepo) {
            this.dataUpRepo = dataUpRepo;
            // this.httpClient = httpClient;
            this.invoiceReadRepo = invoiceReadRepo;
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> CreateJsonFile(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                var json = dataUpRepo.CreateJsonFileAsync(x);
                var dataUpResponse = dataUpRepo.UploadJsonAsync(json);
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = dataUpResponse.Result
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}