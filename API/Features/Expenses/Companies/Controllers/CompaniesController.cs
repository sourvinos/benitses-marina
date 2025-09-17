using API.Infrastructure.Classes;
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

        private readonly ICompanyRepository companyRepo;
        private readonly ICompanyValidation companyValidation;
        private readonly IMapper mapper;

        #endregion

        public CompaniesController(ICompanyRepository companyRepo, ICompanyValidation companyValidation, IMapper mapper) {
            this.companyRepo = companyRepo;
            this.companyValidation = companyValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<CompanyListVM>> GetAsync() {
            return await companyRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CompanyBrowserVM>> GetForBrowserAsync() {
            return await companyRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await companyRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await companyRepo.GetByIdAsync(id, true);
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
            var x = companyValidation.IsValid(null, Company);
            if (x == 200) {
                var z = companyRepo.Create(mapper.Map<CompanyWriteDto, Company>((CompanyWriteDto)companyRepo.AttachMetadataToPostDto(Company)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = companyRepo.GetByIdForBrowserAsync(z.Id).Result,
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
            var x = await companyRepo.GetByIdAsync(Company.Id, false);
            if (x != null) {
                var z = companyValidation.IsValid(x, Company);
                if (z == 200) {
                    companyRepo.Update(mapper.Map<CompanyWriteDto, Company>((CompanyWriteDto)companyRepo.AttachMetadataToPutDto(x, Company)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = companyRepo.GetByIdForBrowserAsync(Company.Id).Result,
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
            var x = await companyRepo.GetByIdAsync(id, false);
            if (x != null) {
                companyRepo.Delete(x);
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