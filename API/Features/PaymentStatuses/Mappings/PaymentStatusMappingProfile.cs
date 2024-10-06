using AutoMapper;

namespace API.Features.Reservations.PaymentStatuses {

    public class PaymentStatusMappingProfile : Profile {

        public PaymentStatusMappingProfile() {
            CreateMap<PaymentStatus, PaymentStatusListVM>();
            CreateMap<PaymentStatus, PaymentStatusBrowserVM>();
            CreateMap<PaymentStatus, PaymentStatusReadDto>();
            CreateMap<PaymentStatusWriteDto, PaymentStatus>();
        }

    }

}