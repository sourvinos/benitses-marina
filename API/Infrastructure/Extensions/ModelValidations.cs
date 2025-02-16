using API.Features.Reservations.Berths;
using API.Infrastructure.Users;
using API.Infrastructure.Account;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions {

    public static class ModelValidations {

        public static void AddModelValidation(IServiceCollection services) {
            // Account
            services.AddTransient<IValidator<ChangePasswordVM>, ChangePasswordValidator>();
            services.AddTransient<IValidator<ForgotPasswordRequestVM>, ForgotPasswordValidator>();
            services.AddTransient<IValidator<ResetPasswordVM>, ResetPasswordValidator>();
            // Reservations
            services.AddTransient<IValidator<BerthWriteDto>, BerthValidator>();
            // Users
            services.AddTransient<IValidator<UserNewDto>, UserNewValidator>();
            services.AddTransient<IValidator<UserUpdateDto>, UserUpdateValidator>();
        }

    }

}