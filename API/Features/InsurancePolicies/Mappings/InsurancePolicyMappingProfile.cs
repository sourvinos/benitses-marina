using API.Features.Reservations;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.InsurancePolicies {

    public class InsurancePolicyMappingProfile : Profile {

        public InsurancePolicyMappingProfile() {
            CreateMap<ReservationInsurance, InsurancePolicyListVM>()
                .ForMember(x => x.PolicyEnds, x => x.MapFrom(x => DateHelpers.DateToISOString(x.PolicyEnds)));
        }

    }

}