using API.Features.Reservations;
using API.Features.Reservations.BoatTypes;
using API.Features.Reservations.Piers;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region reservations
            services.AddTransient<IBoatTypeRepository, BoatTypeRepository>();
            services.AddTransient<IPierRepository, PierRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
            #region validations
            services.AddTransient<IBoatTypeValidation, BoatTypeValidation>();
            services.AddTransient<IPierValidation, PierValidation>();
            services.AddTransient<IReservationValidation, ReservationValidation>();
            services.AddTransient<IUserValidation<IUser>, UserValidation>();
            #endregion
            #region shared
            services.AddScoped<Token>();
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion
        }

    }

}