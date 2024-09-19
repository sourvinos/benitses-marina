using System.Linq;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations.Bookings {

    public class BookingMappingProfile : Profile {

        public BookingMappingProfile() {
            // List
            CreateMap<Booking, BookingListVM>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.Piers, x => x.MapFrom(x => x.BookingPiers.Select(pier => new BookingPierVM {
                    Id = pier.Id,
                    BookingId = pier.BookingId.ToString(),
                    Description = pier.Description
                })));
            // GetById
            CreateMap<Booking, BookingReadDto>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.Piers, x => x.MapFrom(x => x.BookingPiers.Select(pier => new BookingPierVM {
                    Id = pier.Id,
                    BookingId = pier.BookingId.ToString(),
                    Description = pier.Description,
                })));
            // Write
            CreateMap<BookingWriteDto, Booking>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.BoatName.Trim()));
        }

    }

}