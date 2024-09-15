using API.Features.Reservations.Bookings;
using API.Features.Reservations.Piers;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region reservations
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IPierRepository, PierRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
            #region validations
            services.AddTransient<IBookingValidation, BookingValidation>();
            services.AddTransient<IPierValidation, PierValidation>();
            services.AddTransient<IUserValidation<IUser>, UserValidation>();
            #endregion
            #region shared
            services.AddScoped<Token>();
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion
        }

    }

}