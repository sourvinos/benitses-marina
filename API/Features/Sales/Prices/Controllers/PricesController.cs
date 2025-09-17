using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Prices {

    [Route("api/[controller]")]
    public class PricesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPriceRepository priceRepo;
        private readonly IPriceValidation priceValidation;

        #endregion

        public PricesController(IMapper mapper, IPriceRepository priceRepo, IPriceValidation priceValidation) {
            this.mapper = mapper;
            this.priceRepo = priceRepo;
            this.priceValidation = priceValidation;
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PriceListVM>> GetAsync() {
            return await priceRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PriceListBrowserVM>> GetForBrowserAsync() {
            return await priceRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await priceRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Price, PriceReadDto>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("[action]/{code}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByCodeAsync(string code) {
            var x = await priceRepo.GetByCodeAsync(code, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Price, PriceReadDto>(x)
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
        public Response Post([FromBody] PriceWriteDto price) {
            var x = priceValidation.IsValid(null, price);
            if (x == 200) {
                var z = priceRepo.Create(mapper.Map<PriceWriteDto, Price>((PriceWriteDto)priceRepo.AttachMetadataToPostDto(price)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = priceRepo.GetByIdAsync(z.Id, false).Id.ToString(),
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
        public async Task<Response> PutAsync([FromBody] PriceWriteDto price) {
            var x = await priceRepo.GetByIdAsync(price.Id, false);
            if (x != null) {
                var z = priceValidation.IsValid(x, price);
                if (z == 200) {
                    priceRepo.Update(mapper.Map<PriceWriteDto, Price>((PriceWriteDto)priceRepo.AttachMetadataToPutDto(x, price)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = price.Id.ToString(),
                        Message = ApiMessages.OK()
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
            var x = await priceRepo.GetByIdAsync(id, false);
            if (x != null) {
                priceRepo.Delete(x);
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