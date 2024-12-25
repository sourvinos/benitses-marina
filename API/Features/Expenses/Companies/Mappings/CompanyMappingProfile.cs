using AutoMapper;

namespace API.Features.Expenses.Companies {

    public class CompanyMappingProfile : Profile {

        public CompanyMappingProfile() {
            CreateMap<Company, CompanyListVM>();
            CreateMap<Company, CompanyBrowserVM>();
            CreateMap<Company, CompanyReadDto>();
            CreateMap<CompanyWriteDto, Company>();
        }

    }

}