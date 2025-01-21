using API.Features.Sales.Customers;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly ISaleCalculateBalanceRepo calculateBalanceRepo;
        private readonly ISaleEmailSender emailSender;
        private readonly ISaleReadRepository invoiceReadRepo;
        private readonly ISaleUpdateRepository invoiceUpdateRepo;
        private readonly ISaleValidation invoiceValidation;
        private readonly IMapper mapper;

        #endregion

        public InvoicesController(ICustomerRepository customerRepo, ISaleCalculateBalanceRepo calculateBalanceRepo, ISaleEmailSender emailSender, ISaleReadRepository invoiceReadRepo, ISaleUpdateRepository invoiceUpdateRepo, ISaleValidation invoiceValidation, IMapper mapper) {
            this.calculateBalanceRepo = calculateBalanceRepo;
            this.customerRepo = customerRepo;
            this.emailSender = emailSender;
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceUpdateRepo = invoiceUpdateRepo;
            this.invoiceValidation = invoiceValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SaleListVM>> GetAsync() {
            return await invoiceReadRepo.GetAsync();
        }

        [HttpPost("{getForPeriod}")]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SaleListVM>> GetForPeriodAsync([FromBody] SaleListCriteriaVM criteria) {
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
                    Body = mapper.Map<Sale, SaleReadDto>(x)
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
        public async Task<Response> PostAsync([FromBody] SaleCreateDto invoice) {
            invoice.InvoiceNo = await invoiceUpdateRepo.IncreaseInvoiceNoAsync(invoice);
            var x = invoiceValidation.IsValidAsync(null, invoice);
            if (await x == 200) {
                invoice = calculateBalanceRepo.AttachBalancesToCreateDto(invoice, calculateBalanceRepo.CalculateBalances(invoice, invoice.CustomerId, invoice.ShipOwnerId));
                var z = invoiceUpdateRepo.Create(mapper.Map<SaleCreateDto, Sale>((SaleCreateDto)invoiceUpdateRepo.AttachMetadataToPostDto(invoice)));
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

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutAsync([FromBody] InvoiceUpdateDto invoice) {
            var x = await invoiceReadRepo.GetByIdAsync(invoice.InvoiceId.ToString(), false);
            if (x != null) {
                var z = invoiceValidation.IsValidAsync(x, invoice);
                if (await z == 200) {
                    var i = invoiceUpdateRepo.Update(invoice.InvoiceId, mapper.Map<InvoiceUpdateDto, Sale>((InvoiceUpdateDto)invoiceUpdateRepo.AttachMetadataToPutDto(x, invoice)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = i.InvoiceId.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = await z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPut("invoiceAade")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> Put([FromBody] SaleAade invoiceAade) {
            var x = await invoiceReadRepo.GetInvoiceAadeByIdAsync(invoiceAade.InvoiceId.ToString());
            if (x != null) {
                invoiceUpdateRepo.UpdateInvoiceAade(invoiceAade);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = invoiceAade.InvoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchInvoicesWithEmailPending([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    invoiceUpdateRepo.UpdateIsEmailPending(x, invoiceId);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchInvoicesWithEmailSent([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    invoiceUpdateRepo.UpdateIsEmailSent(x, invoiceId);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpPatch("isCancelled/{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchIsCancelled(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, false);
            if (x != null) {
                invoiceUpdateRepo.UpdateIsCancelled(x, invoiceId);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = invoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, false);
            if (x != null) {
                invoiceUpdateRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.InvoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}