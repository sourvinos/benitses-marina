using API.Features.Reservations;
using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;
using API.Features.InsurancePolicies;
using API.Features.BoatTypes;
using API.Features.BoatUsages;
using API.Features.Leases;
using API.Features.Expenses.Banks;
using API.Features.Expenses.Expenses;
using API.Features.Expenses.PaymentMethods;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Expenses.DocumentTypes;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region expenses
            services.AddTransient<IBankRepository, BankRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
            #endregion
            #region reservations
            services.AddTransient<IBerthRepository, BerthRepository>();
            services.AddTransient<IBoatTypeRepository, BoatTypeRepository>();
            services.AddTransient<IBoatUsageRepository, BoatUsageRepository>();
            services.AddTransient<IInsurancePolicyRepository, InsurancePolicyRepository>();
            services.AddTransient<ILeasePdfRepository, LeasePdfRepository>();
            services.AddTransient<ILeaseRepository, LeaseRepository>();
            services.AddTransient<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
            #region validations
            services.AddTransient<IBankValidation, BankValidation>();
            services.AddTransient<IBerthValidation, BerthValidation>();
            services.AddTransient<IBoatTypeValidation, BoatTypeValidation>();
            services.AddTransient<IBoatUsageValidation, BoatUsageValidation>();
            services.AddTransient<IDocumentTypeValidation, DocumentTypeValidation>();
            services.AddTransient<IExpenseValidation, ExpenseValidation>();
            services.AddTransient<IPaymentMethodValidation, PaymentMethodValidation>();
            services.AddTransient<IPaymentStatusValidation, PaymentStatusValidation>();
            services.AddTransient<IReservationValidation, ReservationValidation>();
            services.AddTransient<ISupplierValidation, SupplierValidation>();
            services.AddTransient<IUserValidation<IUser>, UserValidation>();
            #endregion
            #region shared
            services.AddScoped<Token>();
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion
        }

    }

}