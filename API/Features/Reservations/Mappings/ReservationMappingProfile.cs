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
                .ForMember(x => x.Owner, x => x.MapFrom(x => new ReservationListOwnerVM {
                    Id = x.Owner.Id,
                    ReservationId = x.Owner.ReservationId.ToString(),
                    Name = x.Owner.Name
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
                .ForMember(x => x.Boat, x => x.MapFrom(x => new ReservationBoatDetailsDto {
                    Id = x.Boat.Id,
                    ReservationId = x.Boat.ReservationId,
                    Name = x.Boat.Name,
                    Flag = x.Boat.Flag,
                    Loa = x.Boat.Loa,
                    Beam = x.Boat.Beam,
                    Draft = x.Boat.Draft,
                    RegistryPort = x.Boat.RegistryPort,
                    RegistryNo = x.Boat.RegistryNo,
                    Type = x.Boat.Type,
                    Usage = x.Boat.Usage
                }))
                .ForMember(x => x.Insurance, x => x.MapFrom(x => new ReservationInsuranceDetailsDto {
                    Id = x.Insurance.Id,
                    ReservationId = x.Insurance.ReservationId,
                    InsuranceCompany = x.Insurance.InsuranceCompany,
                    PolicyNo = x.Insurance.PolicyNo,
                    PolicyEnds = DateHelpers.DateToISOString(x.Insurance.PolicyEnds)
                }))
                .ForMember(x => x.Owner, x => x.MapFrom(x => new ReservationOwnerDetailsDto {
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
                .ForMember(x => x.Billing, x => x.MapFrom(x => new ReservationBillingDetailsDto {
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
                .ForMember(x => x.Fee, x => x.MapFrom(x => new ReservationFeeDetailsDto {
                    Id = x.Fee.Id,
                    ReservationId = x.Fee.ReservationId,
                    NetAmount = x.Fee.NetAmount,
                    VatAmount = x.Fee.VatAmount,
                    GrossAmount = x.Fee.GrossAmount
                }))
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
                .ForMember(x => x.Boat, x => x.MapFrom(x => new ReservationBoatDetails {
                    ReservationId = x.ReservationId,
                    Name = x.Boat.Name,
                    Loa = x.Boat.Loa,
                    Beam = x.Boat.Beam,
                    Draft = x.Boat.Draft,
                    Type = x.Boat.Type,
                    Usage = x.Boat.Usage
                }))
                .ForMember(x => x.Insurance, x => x.MapFrom(x => new ReservationInsuranceDetails {
                    ReservationId = x.ReservationId,
                    InsuranceCompany = x.Insurance.InsuranceCompany,
                    PolicyNo = x.Insurance.PolicyNo,
                    PolicyEnds = new DateTime(2199, 12, 31),
                }))
                .ForMember(x => x.Owner, x => x.MapFrom(x => new ReservationOwnerDetails {
                    ReservationId = x.ReservationId,
                    Name = x.Owner.Name,
                    Address = x.Owner.Address,
                    TaxNo = x.Owner.TaxNo,
                    TaxOffice = x.Owner.TaxOffice,
                    PassportNo = x.Owner.PassportNo,
                    Phones = x.Owner.Phones,
                    Email = x.Owner.Email,
                }))
                .ForMember(x => x.Billing, x => x.MapFrom(x => new ReservationBillingDetails {
                    ReservationId = x.ReservationId,
                    Name = x.Billing.Name,
                    Address = x.Billing.Address,
                    TaxNo = x.Billing.TaxNo,
                    TaxOffice = x.Billing.TaxOffice,
                    PassportNo = x.Billing.PassportNo,
                    Phones = x.Billing.Phones,
                    Email = x.Billing.Email,
                }))
                .ForMember(x => x.Fee, x => x.MapFrom(x => new ReservationFeeDetails {
                    ReservationId = x.ReservationId,
                    NetAmount = x.Fee.NetAmount,
                    VatAmount = x.Fee.VatAmount,
                    GrossAmount = x.Fee.GrossAmount
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.FinancialRemarks, x => x.MapFrom(x => x.FinancialRemarks.Trim()))
;            // Write berth
            CreateMap<ReservationBerthWriteDto, ReservationBerth>();
        }

    }

}