using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class SalesController : ControllerBase {

        #region variables

        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IInvoiceUpdateRepository invoiceUpdateRepo;
        private readonly IInvoiceValidation invoiceValidation;
        private readonly IMapper mapper;

        #endregion

        public SalesController(IInvoiceReadRepository invoiceReadRepo, IInvoiceUpdateRepository invoiceUpdateRepo, IInvoiceValidation invoiceValidation, IMapper mapper) {
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceUpdateRepo = invoiceUpdateRepo;
            this.invoiceValidation = invoiceValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceistVM>> GetAsync() {
            return await invoiceReadRepo.GetAsync();
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Invoice, InvoiceReadDto>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PostAsync([FromBody] InvoiceCreateDto invoice) {
            invoice.InvoiceNo = await invoiceUpdateRepo.IncreaseInvoiceNoAsync(invoice);
            var x = invoiceValidation.IsValidAsync(null, invoice);
            if (await x == 200) {
                var z = invoiceUpdateRepo.Create(mapper.Map<InvoiceCreateDto, Invoice>((InvoiceCreateDto)invoiceUpdateRepo.AttachMetadataToPostDto(invoice)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = z.InvoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = await x
                };
            }
        }

    }

}