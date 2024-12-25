using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Companies {

    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly ICompanyRepository CompanyRepo;
        private readonly ICompanyValidation CompanyValidation;

        #endregion

        public CompaniesController(IMapper mapper, ICompanyRepository CompanyRepo, ICompanyValidation CompanyValidation) {
            this.mapper = mapper;
            this.CompanyRepo = CompanyRepo;
            this.CompanyValidation = CompanyValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<CompanyListVM>> GetAsync() {
            return await CompanyRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CompanyBrowserVM>> GetForBrowserAsync() {
            return await CompanyRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await CompanyRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Company, CompanyReadDto>(x),
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
        public ResponseWithBody Post([FromBody] CompanyWriteDto Company) {
            var x = CompanyValidation.IsValid(null, Company);
            if (x == 200) {
                var z = CompanyRepo.Create(mapper.Map<CompanyWriteDto, Company>((CompanyWriteDto)CompanyRepo.AttachMetadataToPostDto(Company)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = CompanyRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] CompanyWriteDto Company) {
            var x = await CompanyRepo.GetByIdAsync(Company.Id, false);
            if (x != null) {
                var z = CompanyValidation.IsValid(x, Company);
                if (z == 200) {
                    CompanyRepo.Update(mapper.Map<CompanyWriteDto, Company>((CompanyWriteDto)CompanyRepo.AttachMetadataToPutDto(x, Company)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = CompanyRepo.GetByIdForBrowserAsync(Company.Id).Result,
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
            var x = await CompanyRepo.GetByIdAsync(id, false);
            if (x != null) {
                CompanyRepo.Delete(x);
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