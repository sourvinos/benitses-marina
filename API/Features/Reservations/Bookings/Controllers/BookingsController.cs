using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Bookings {

    [Route("api/[controller]")]
    public class BookingsController : ControllerBase {

        #region variables

        private readonly IBookingRepository bookingRepo;
        private readonly IBookingValidation bookingValidation;
        private readonly IMapper mapper;

        #endregion

        public BookingsController(IBookingRepository bookingRepo, IBookingValidation bookingValidation, IMapper mapper) {
            this.mapper = mapper;
            this.bookingRepo = bookingRepo;
            this.bookingValidation = bookingValidation;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BookingListVM>> GetAsync() {
            return await bookingRepo.GetAsync();
        }

        [HttpGet("{bookingId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string bookingId) {
            var x = await bookingRepo.GetByIdAsync(bookingId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Booking, BookingReadDto>(x)
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
        public Response Post([FromBody] BookingWriteDto booking) {
            var z = bookingValidation.IsValid(null, booking);
            if (z == 200) {
                var x = bookingRepo.Create(mapper.Map<BookingWriteDto, Booking>((BookingWriteDto)bookingRepo.AttachMetadataToPostDto(booking)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.BookingId.ToString(),
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
        public async Task<Response> PutAsync([FromBody] BookingWriteDto booking) {
            var x = await bookingRepo.GetByIdAsync(booking.BookingId.ToString(), false);
            if (x != null) {
                var z = bookingValidation.IsValid(x, booking);
                if (z == 200) {
                    bookingRepo.Update(booking.BookingId, mapper.Map<BookingWriteDto, Booking>((BookingWriteDto)bookingRepo.AttachMetadataToPutDto(x, booking)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.BookingId.ToString(),
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
            var x = await bookingRepo.GetByIdAsync(id, false);
            if (x != null) {
                bookingRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.BookingId.ToString(),
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