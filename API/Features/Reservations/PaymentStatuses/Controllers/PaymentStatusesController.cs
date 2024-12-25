using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.PaymentStatuses {

    [Route("api/[controller]")]
    public class PaymentStatusesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPaymentStatusRepository paymentStatusRepo;
        private readonly IPaymentStatusValidation paymentStatusValidation;

        #endregion

        public PaymentStatusesController(IMapper mapper, IPaymentStatusRepository paymentStatusRepo, IPaymentStatusValidation paymentStatusValidation) {
            this.mapper = mapper;
            this.paymentStatusRepo = paymentStatusRepo;
            this.paymentStatusValidation = paymentStatusValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PaymentStatusListVM>> GetAsync() {
            return await paymentStatusRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PaymentStatusBrowserVM>> GetForBrowserAsync() {
            return await paymentStatusRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await paymentStatusRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<PaymentStatus, PaymentStatusReadDto>(x),
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
        public ResponseWithBody Post([FromBody] PaymentStatusWriteDto paymentStatus) {
            var x = paymentStatusValidation.IsValid(null, paymentStatus);
            if (x == 200) {
                var z = paymentStatusRepo.Create(mapper.Map<PaymentStatusWriteDto, PaymentStatus>((PaymentStatusWriteDto)paymentStatusRepo.AttachMetadataToPostDto(paymentStatus)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = paymentStatusRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] PaymentStatusWriteDto PaymentStatus) {
            var x = await paymentStatusRepo.GetByIdAsync(PaymentStatus.Id, false);
            if (x != null) {
                var z = paymentStatusValidation.IsValid(x, PaymentStatus);
                if (z == 200) {
                    paymentStatusRepo.Update(mapper.Map<PaymentStatusWriteDto, PaymentStatus>((PaymentStatusWriteDto)paymentStatusRepo.AttachMetadataToPutDto(x, PaymentStatus)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = paymentStatusRepo.GetByIdForBrowserAsync(PaymentStatus.Id).Result,
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
            var x = await paymentStatusRepo.GetByIdAsync(id, false);
            if (x != null) {
                paymentStatusRepo.Delete(x);
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