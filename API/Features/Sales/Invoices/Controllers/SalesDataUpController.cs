using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class SalesDataUpController : ControllerBase {

        #region variables

        private readonly AppDbContext context;
        private readonly IInvoiceDataUpRepository dataUpRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;

        #endregion

        public SalesDataUpController(AppDbContext context, IInvoiceReadRepository invoiceReadRepo, IInvoiceDataUpRepository dataUpRepo) {
            this.context = context;
            this.dataUpRepo = dataUpRepo;
            this.invoiceReadRepo = invoiceReadRepo;
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                var response = dataUpRepo.UploadJsonInvoiceAsync(dataUpRepo.SaveJsonInvoice(dataUpRepo.CreateJsonInvoice(this.context.Companies.First(x => x.TaxNo == "801515394"), x)), this.context.Companies.First(x => x.TaxNo == "801515394"));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = dataUpRepo.ShowResponseAfterUploadJsonInvoice(response.Result)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}