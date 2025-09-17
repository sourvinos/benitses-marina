using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Common.PaymentMethods {

    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPaymentMethodRepository paymentMethodRepo;
        private readonly IPaymentMethodValidation paymentMethodValidation;

        #endregion

        public PaymentMethodsController(IMapper mapper, IPaymentMethodRepository paymentMethodRepo, IPaymentMethodValidation paymentMethodValidation) {
            this.mapper = mapper;
            this.paymentMethodRepo = paymentMethodRepo;
            this.paymentMethodValidation = paymentMethodValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PaymentMethodListVM>> GetAsync() {
            return await paymentMethodRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PaymentMethodBrowserVM>> GetForBrowserAsync() {
            return await paymentMethodRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await paymentMethodRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<PaymentMethod, PaymentMethodReadDto>(x),
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
        public ResponseWithBody Post([FromBody] PaymentMethodWriteDto paymentMethod) {
            var x = paymentMethodValidation.IsValid(null, paymentMethod);
            if (x == 200) {
                var z = paymentMethodRepo.Create(mapper.Map<PaymentMethodWriteDto, PaymentMethod>((PaymentMethodWriteDto)paymentMethodRepo.AttachMetadataToPostDto(paymentMethod)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = paymentMethodRepo.GetByIdForBrowserAsync(z.Id).Result,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = x
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<ResponseWithBody> Put([FromBody] PaymentMethodWriteDto paymentMethod) {
            var x = await paymentMethodRepo.GetByIdAsync(paymentMethod.Id, false);
            if (x != null) {
                var z = paymentMethodValidation.IsValid(x, paymentMethod);
                if (z == 200) {
                    paymentMethodRepo.Update(mapper.Map<PaymentMethodWriteDto, PaymentMethod>((PaymentMethodWriteDto)paymentMethodRepo.AttachMetadataToPutDto(x, paymentMethod)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = paymentMethodRepo.GetByIdForBrowserAsync(paymentMethod.Id).Result,
                        Message = ApiMessages.OK(),
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] int id) {
            var x = await paymentMethodRepo.GetByIdAsync(id, false);
            if (x != null) {
                paymentMethodRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.Id.ToString(),
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