using AutoMapper;

namespace API.Features.Expenses.Banks {

    public class BankMappingProfile : Profile {

        public BankMappingProfile() {
            CreateMap<Bank, BankListVM>();
            CreateMap<Bank, BankBrowserVM>();
            CreateMap<Bank, BankReadDto>();
            CreateMap<BankWriteDto, Bank>();
        }

    }

}