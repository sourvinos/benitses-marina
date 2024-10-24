using System;
using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations {

    public class ReservationMappingProfile : Profile {

        public ReservationMappingProfile() {
            // List
            CreateMap<Reservation, ReservationListVM>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.Berths, x => x.MapFrom(x => x.Berths.Select(berth => new ReservationBerthVM {
                    Id = berth.Id,
                    ReservationId = berth.ReservationId.ToString(),
                    Description = berth.Description
                })))
                .ForMember(x => x.PaymentStatus, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentStatus.Id,
                    Description = x.PaymentStatus.Description
                }))
                .ForMember(x => x.IsOverdue, x => x.MapFrom(x => ReservationHelpers.IsOverdue(x.ToDate)));
            // GetById
            CreateMap<Reservation, ReservationReadDto>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.Berths, x => x.MapFrom(x => x.Berths.Select(berth => new ReservationBerthVM {
                    Id = berth.Id,
                    ReservationId = berth.ReservationId.ToString(),
                    Description = berth.Description
                })))
                .ForMember(x => x.PaymentStatus, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentStatus.Id,
                    Description = x.PaymentStatus.Description
                }));
            // Write reservation
            CreateMap<ReservationWriteDto, Reservation>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.BoatName.Trim()))
                .ForMember(x => x.Customer, x => x.MapFrom(x => x.Customer.Trim()))
                .ForMember(x => x.Email, x => x.MapFrom(x => x.Email.Trim()))
                .ForMember(x => x.Contact, x => x.MapFrom(x => x.Contact.Trim()))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.FinancialRemarks, x => x.MapFrom(x => x.FinancialRemarks.Trim()))
                .ForMember(x => x.ReservationLease, x => x.MapFrom(x => new ReservationLease {
                    ReservationId = x.ReservationId,
                    InsuranceCompany = x.ReservationLease.InsuranceCompany,
                    PolicyNo = x.ReservationLease.PolicyNo,
                    PolicyEnds = new DateTime(2199, 12, 31),
                }))
;            // Write berth
            CreateMap<ReservationBerthWriteDto, ReservationBerth>();
        }

    }

}