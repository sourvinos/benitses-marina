using API.Features.Reservations;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;
using API.Features.LeaseAgreements;
using API.Features.InsurancePolicies;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region reservations
            services.AddTransient<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddTransient<IBerthRepository, BerthRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<ILeaseAgreementRepository, LeaseAgreementRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IInsurancePolicyRepository, InsurancePolicyRepository>();
            #endregion
            #region validations
            services.AddTransient<IPaymentStatusValidation, PaymentStatusValidation>();
            services.AddTransient<IBerthValidation, BerthValidation>();
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