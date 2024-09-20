using System.Collections.Generic;
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

        private readonly IReservationRepository ReservationRepo;
        private readonly IReservationValidation ReservationValidation;
        private readonly IMapper mapper;

        #endregion

        public ReservationsController(IReservationRepository ReservationRepo, IReservationValidation ReservationValidation, IMapper mapper) {
            this.mapper = mapper;
            this.ReservationRepo = ReservationRepo;
            this.ReservationValidation = ReservationValidation;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ReservationListVM>> GetAsync() {
            return await ReservationRepo.GetAsync();
        }

        [HttpGet("{ReservationId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string ReservationId) {
            var x = await ReservationRepo.GetByIdAsync(ReservationId, true);
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
        public Response Post([FromBody] ReservationWriteDto Reservation) {
            var z = ReservationValidation.IsValid(null, Reservation);
            if (z == 200) {
                var x = ReservationRepo.Create(mapper.Map<ReservationWriteDto, Reservation>((ReservationWriteDto)ReservationRepo.AttachMetadataToPostDto(Reservation)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = z
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutAsync([FromBody] ReservationWriteDto Reservation) {
            var x = await ReservationRepo.GetByIdAsync(Reservation.ReservationId.ToString(), false);
            if (x != null) {
                var z = ReservationValidation.IsValid(x, Reservation);
                if (z == 200) {
                    ReservationRepo.Update(Reservation.ReservationId, mapper.Map<ReservationWriteDto, Reservation>((ReservationWriteDto)ReservationRepo.AttachMetadataToPutDto(x, Reservation)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.ReservationId.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = z
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
            var x = await ReservationRepo.GetByIdAsync(id, false);
            if (x != null) {
                ReservationRepo.Delete(x);
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