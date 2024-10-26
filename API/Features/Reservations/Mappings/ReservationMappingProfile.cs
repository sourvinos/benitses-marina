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
                .ForMember(x => x.ReservationLease, x => x.MapFrom(x => new ReservationListLeaseVM {
                    Customer = x.ReservationOwner.Owner
                }))
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
                }))
                .ForMember(x => x.ReservationLease, x => x.MapFrom(x => new ReservationLeaseReadDto {
                    Id = x.ReservationLease.Id,
                    ReservationId = x.ReservationLease.ReservationId,
                    InsuranceCompany = x.ReservationLease.InsuranceCompany,
                    PolicyNo = x.ReservationLease.PolicyNo,
                    PolicyEnds = DateHelpers.DateToISOString(x.ReservationLease.PolicyEnds),
                    Flag = x.ReservationLease.Flag,
                    RegistryPort = x.ReservationLease.RegistryPort,
                    RegistryNo = x.ReservationLease.RegistryNo,
                    BoatType = x.ReservationLease.BoatType,
                    BoatUsage = x.ReservationLease.BoatUsage,
                    NetAmount = x.ReservationLease.NetAmount,
                    VatAmount = x.ReservationLease.VatAmount,
                    GrossAmount = x.ReservationLease.GrossAmount
                }))
                .ForMember(x => x.ReservationOwner, x => x.MapFrom(x => new ReservationOwnerReadDto {
                    Id = x.ReservationOwner.Id,
                    ReservationId = x.ReservationOwner.ReservationId,
                    Owner = x.ReservationOwner.Owner,
                    Address = x.ReservationOwner.Address,
                    TaxNo = x.ReservationOwner.TaxNo,
                    TaxOffice = x.ReservationOwner.TaxOffice,
                    PassportNo = x.ReservationOwner.PassportNo,
                    Phones = x.ReservationOwner.Phones,
                    Email = x.ReservationOwner.Email
                }));
            // Write reservation
            CreateMap<ReservationWriteDto, Reservation>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.BoatName.Trim()))
                .ForMember(x => x.Beam, x => x.MapFrom(x => x.Beam ?? ""))
                .ForMember(x => x.Draft, x => x.MapFrom(x => x.Draft ?? ""))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.FinancialRemarks, x => x.MapFrom(x => x.FinancialRemarks.Trim()))
                .ForMember(x => x.ReservationOwner, x => x.MapFrom(x => new ReservationOwner {
                    ReservationId = x.ReservationId,
                    Owner = x.ReservationOwner.Owner,
                    Address = x.ReservationOwner.Address,
                    TaxNo = x.ReservationOwner.TaxNo,
                    TaxOffice = x.ReservationOwner.TaxOffice,
                    PassportNo = x.ReservationOwner.PassportNo,
                    Phones = x.ReservationOwner.Phones,
                    Email = x.ReservationOwner.Email,
                }))
                .ForMember(x => x.ReservationLease, x => x.MapFrom(x => new ReservationLease {
                    ReservationId = x.ReservationId,
                    InsuranceCompany = x.ReservationLease.InsuranceCompany,
                    PolicyNo = x.ReservationLease.PolicyNo,
                    PolicyEnds = new DateTime(2199, 12, 31),
                    Flag = x.ReservationLease.Flag,
                    RegistryPort = x.ReservationLease.RegistryPort,
                    RegistryNo = x.ReservationLease.RegistryNo,
                    BoatType = x.ReservationLease.BoatType,
                    BoatUsage = x.ReservationLease.BoatUsage,
                    NetAmount = x.ReservationLease.NetAmount,
                    VatAmount = x.ReservationLease.VatAmount,
                    GrossAmount = x.ReservationLease.GrossAmount
                }))
;            // Write berth
            CreateMap<ReservationBerthWriteDto, ReservationBerth>();
        }

    }

}