﻿using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations {

    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase {

        #region variables

        private readonly IReservationRepository reservationRepo;
        private readonly IReservationValidation reservationValidation;
        private readonly IMapper mapper;

        #endregion

        public ReservationsController(IReservationRepository reservationRepo, IReservationValidation reservationValidation, IMapper mapper) {
            this.mapper = mapper;
            this.reservationRepo = reservationRepo;
            this.reservationValidation = reservationValidation;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ReservationListVM>> GetAsync() {
            return await reservationRepo.GetAsync();
        }

        [HttpGet("arrivals/{date}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ReservationListVM>> GetArrivalsAsync(string date) {
            return await reservationRepo.GetArrivalsAsync(date);
        }

        [HttpGet("departures/{date}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ReservationListVM>> GetDeparturesAsync(string date) {
            return await reservationRepo.GetDeparturesAsync(date);
        }

        [HttpGet("{reservationId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string reservationId) {
            var x = await reservationRepo.GetByIdAsync(reservationId, true);
            if (x != null) {
                return new ResponseWithBody {   
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Reservation, ReservationReadDto>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PostAsync([FromBody] ReservationWriteDto reservation) {
            var z = reservationValidation.IsValidAsync(null, reservation);
            if (await z == 200) {
                var x = reservationRepo.Create(mapper.Map<ReservationWriteDto, Reservation>((ReservationWriteDto)reservationRepo.AttachMetadataToPostDto(reservation)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = await z
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutAsync([FromBody] ReservationWriteDto reservation) {
            var x = await reservationRepo.GetByIdAsync(reservation.ReservationId.ToString(), false);
            if (x != null) {
                var z = reservationValidation.IsValidAsync(x, reservation);
                if (await z == 200) {
                    reservationRepo.Update(reservation.ReservationId, mapper.Map<ReservationWriteDto, Reservation>((ReservationWriteDto)reservationRepo.AttachMetadataToPutDto(x, reservation)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.ReservationId.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = await z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string id) {
            var x = await reservationRepo.GetByIdAsync(id, false);
            if (x != null) {
                reservationRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}