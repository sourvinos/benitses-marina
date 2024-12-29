using AutoMapper;

namespace API.Features.Expenses.BalanceFilters {

    public class BalanceFilterMappingProfile : Profile {

        public BalanceFilterMappingProfile() {
            CreateMap<BalanceFilter, BalanceFilterBrowserVM>();
        }

    }

}