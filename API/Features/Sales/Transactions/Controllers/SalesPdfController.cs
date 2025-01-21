using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    [Route("api/[controller]")]
    public class InvoicesPdfController : ControllerBase {

        #region variables

        private readonly ISalePdfRepository invoicePdfRepo;
        private readonly ISaleReadRepository invoiceReadRepo;
        private readonly IMapper mapper;

        #endregion

        public InvoicesPdfController(ISalePdfRepository invoicePdfRepo, ISaleReadRepository invoiceReadRepo, IMapper mapper) {
            this.invoicePdfRepo = invoicePdfRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.mapper = mapper;
        }

        [HttpPost("buildInvoicePdfs")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> BuildInvoicePdfsAsync([FromBody] string[] invoiceIds) {
            var filenames = new List<string>();
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPdfAsync(invoiceId);
                if (x != null) {
                    var z = invoicePdfRepo.BuildPdf(mapper.Map<Sale, SalePdfVM>(x));
                    filenames.Add(z);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = filenames.ToArray()
            };
        }

        [HttpPost("buildMultiPagePdf")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> BuildMultiPagePdfAsync([FromBody] string[] invoiceIds) {
            var invoices = new List<SalePdfVM>();
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPdfAsync(invoiceId);
                if (x != null) {
                    invoices.Add(mapper.Map<Sale, SalePdfVM>(x));
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            var filename = invoicePdfRepo.BuildMultiPagePdf(invoices);
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = filename
            };
        }

        [HttpGet("[action]/{filename}")]
        [Authorize(Roles = "admin")]
        public IActionResult OpenPdf([FromRoute] string filename) {
            return invoicePdfRepo.OpenPdf(filename);
        }

    }

}