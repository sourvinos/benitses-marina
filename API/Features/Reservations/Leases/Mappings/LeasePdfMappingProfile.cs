using API.Features.Reservations.Transactions;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Leases {

    public class LeasePdfMappingProfile : Profile {

        public LeasePdfMappingProfile() {
            CreateMap<Reservation, LeasePdfVM>()
                .ForMember(x => x.Period, x => x.MapFrom(x => new LeasePdfPeriodVM {
                    ReservationId = x.ReservationId,
                    FromDate = DateHelpers.DateToISOString(x.FromDate),
                    ToDate = DateHelpers.DateToISOString(x.ToDate)
                }))
                .ForMember(x => x.Boat, x => x.MapFrom(x => new LeasePdfBoatVM {
                    Id = x.Boat.Id,
                    ReservationId = x.Boat.ReservationId,
                    Type = new SimpleEntity { Id = x.Boat.Type.Id, Description = x.Boat.Type.Description },
                    Usage = new SimpleEntity { Id = x.Boat.Usage.Id, Description = x.Boat.Usage.Description },
                    Name = x.Boat.Name,
                    Flag = x.Boat.Flag,
                    Loa = x.Boat.Loa,
                    Beam = x.Boat.Beam,
                    Draft = x.Boat.Draft,
                    RegistryPort = x.Boat.RegistryPort,
                    RegistryNo = x.Boat.RegistryNo,
                }))
                .ForMember(x => x.Insurance, x => x.MapFrom(x => new LeasePdfInsuranceVM {
                    Id = x.Insurance.Id,
                    ReservationId = x.Insurance.ReservationId,
                    InsuranceCompany = x.Insurance.InsuranceCompany,
                    PolicyNo = x.Insurance.PolicyNo,
                    PolicyEnds = DateHelpers.DateToISOString(x.Insurance.PolicyEnds)
                }))
                .ForMember(x => x.Owner, x => x.MapFrom(x => new LeasePdfPersonVM {
                    Id = x.Owner.Id,
                    ReservationId = x.Owner.ReservationId,
                    Name = x.Owner.Name,
                    Address = x.Owner.Address,
                    TaxNo = x.Owner.TaxNo,
                    TaxOffice = x.Owner.TaxOffice,
                    PassportNo = x.Owner.PassportNo,
                    Phones = x.Owner.Phones,
                    Email = x.Owner.Email
                }))
                .ForMember(x => x.Billing, x => x.MapFrom(x => new LeasePdfPersonVM {
                    Id = x.Billing.Id,
                    ReservationId = x.Billing.ReservationId,
                    Name = x.Billing.Name,
                    Address = x.Billing.Address,
                    TaxNo = x.Billing.TaxNo,
                    TaxOffice = x.Billing.TaxOffice,
                    PassportNo = x.Billing.PassportNo,
                    Phones = x.Billing.Phones,
                    Email = x.Billing.Email
                }))
                .ForMember(x => x.Fee, x => x.MapFrom(x => new LeasePdfFeeVM {
                    Id = x.Fee.Id,
                    ReservationId = x.Fee.ReservationId,
                    NetAmount = x.Fee.NetAmount,
                    VatPercent = x.Fee.VatPercent,
                    VatAmount = x.Fee.VatAmount,
                    GrossAmount = x.Fee.GrossAmount
                }));
        }

    }

}