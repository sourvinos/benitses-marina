using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Sales.Customers;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class SalesController : ControllerBase {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly IInvoiceCreateRepository invoiceUpdateRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IInvoiceValidation invoiceValidation;
        private readonly IMapper mapper;

        #endregion

        public SalesController(ICustomerRepository customerRepo, IInvoiceCreateRepository invoiceUpdateRepo, IInvoiceReadRepository invoiceReadRepo, IInvoiceValidation invoiceValidation, IMapper mapper) {
            this.customerRepo = customerRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceUpdateRepo = invoiceUpdateRepo;
            this.invoiceValidation = invoiceValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetAsync() {
            return await invoiceReadRepo.GetAsync();
        }

        [HttpPost("{getForPeriod}")]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetForPeriodAsync([FromBody] SaleListCriteriaVM criteria) {
            return await invoiceReadRepo.GetForPeriodAsync(criteria);
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
            invoice.Date = invoiceUpdateRepo.GetToday();
            var x = invoiceValidation.IsValidAsync(null, invoice);
            if (await x == 200) {
                var i = customerRepo.GetByIdAsync(invoice.CustomerId, false).Result;
                foreach (var item in invoice.Items) {
                    item.TaxCode = i.VatPercentId;
                    item.TaxException = i.VatExemptionId;
                }
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