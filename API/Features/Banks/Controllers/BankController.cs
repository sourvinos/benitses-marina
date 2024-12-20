using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Banks {

    [Route("api/[controller]")]
    public class BanksController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IBankRepository bankRepo;
        private readonly IBankValidation bankValidation;

        #endregion

        public BanksController(IMapper mapper, IBankRepository bankRepo, IBankValidation BankValidation) {
            this.mapper = mapper;
            this.bankRepo = bankRepo;
            this.bankValidation = BankValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BankListVM>> GetAsync() {
            return await bankRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BankBrowserVM>> GetForBrowserAsync() {
            return await bankRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await bankRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Bank, BankReadDto>(x),
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
        public ResponseWithBody Post([FromBody] BankWriteDto Bank) {
            var x = bankValidation.IsValid(null, Bank);
            if (x == 200) {
                var z = bankRepo.Create(mapper.Map<BankWriteDto, Bank>((BankWriteDto)bankRepo.AttachMetadataToPostDto(Bank)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = bankRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] BankWriteDto Bank) {
            var x = await bankRepo.GetByIdAsync(Bank.Id, false);
            if (x != null) {
                var z = bankValidation.IsValid(x, Bank);
                if (z == 200) {
                    bankRepo.Update(mapper.Map<BankWriteDto, Bank>((BankWriteDto)bankRepo.AttachMetadataToPutDto(x, Bank)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = bankRepo.GetByIdForBrowserAsync(Bank.Id).Result,
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
            var x = await bankRepo.GetByIdAsync(id, false);
            if (x != null) {
                bankRepo.Delete(x);
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