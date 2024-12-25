using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Expenses.Companies {

    public class CompanyMappingProfile : Profile {

        public CompanyMappingProfile() {
            CreateMap<Company, CompanyListVM>();
            CreateMap<Company, CompanyBrowserVM>();
            CreateMap<Company, SimpleEntity>();
            CreateMap<Company, CompanyReadDto>();
            CreateMap<CompanyWriteDto, Company>();
        }

    }

}