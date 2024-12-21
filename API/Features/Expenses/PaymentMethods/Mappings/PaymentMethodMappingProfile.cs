using AutoMapper;

namespace API.Features.Expenses.PaymentMethods {

    public class PaymentMethodMappingProfile : Profile {

        public PaymentMethodMappingProfile() {
            CreateMap<PaymentMethod, PaymentMethodListVM>();
            CreateMap<PaymentMethod, PaymentMethodBrowserVM>();
            CreateMap<PaymentMethod, PaymentMethodReadDto>();
            CreateMap<PaymentMethodWriteDto, PaymentMethod>();
        }

    }

}