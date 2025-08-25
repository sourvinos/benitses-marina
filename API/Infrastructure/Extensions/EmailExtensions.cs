using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions {

    public static class Email {

        public static IServiceCollection AddEmailSenders(this IServiceCollection services) {

            return services;

        }

    }

}