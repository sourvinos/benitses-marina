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
using API.Features.Expenses.PaymentMethods;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.Ledgers;
using API.Features.Expenses.Companies;
using API.Features.Expenses.BalanceSheet;
using API.Features.Expenses.BalanceFilters;
using API.Features.Sales.Customers;
using API.Features.Sales.Nationalities;
using API.Features.Sales.TaxOffices;
using API.Features.Sales.Prices;
using API.Features.Expenses.Statistics;
using API.Features.Expenses.Transactions;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region expenses
            services.AddTransient<IBalanceFilterRepository, BalanceFilterRepository>();
            services.AddTransient<IBalanceSheetRepository, BalanceSheetRepository>();
            services.AddTransient<IBankRepository, BankRepository>();
            services.AddTransient<IBankValidation, BankValidation>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyValidation, CompanyValidation>();
            services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddTransient<IDocumentTypeValidation, DocumentTypeValidation>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IExpenseValidation, ExpenseValidation>();
            services.AddTransient<ILedgerRepository, LedgerRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IPaymentMethodValidation, PaymentMethodValidation>();
            services.AddTransient<IPaymentStatusValidation, PaymentStatusValidation>();
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ISupplierValidation, SupplierValidation>();
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
            #region Sales
            services.AddTransient<ICustomerAadeRepository, CustomerAadeRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerValidation, CustomerValidation>();
            services.AddTransient<INationalityRepository, NationalityRepository>();
            services.AddTransient<ITaxOfficeRepository, TaxOfficeRepository>();
            services.AddTransient<ITaxOfficeValidation, TaxOfficeValidation>();
            #endregion
            #region validations
            services.AddTransient<IBerthValidation, BerthValidation>();
            services.AddTransient<IBoatTypeValidation, BoatTypeValidation>();
            services.AddTransient<IBoatUsageValidation, BoatUsageValidation>();
            services.AddTransient<IReservationValidation, ReservationValidation>();
            #endregion
            #region sales
            services.AddTransient<IPriceRepository, PriceRepository>();
            services.AddTransient<IPriceValidation, PriceValidation>();
            #endregion
            #region shared
            services.AddScoped<Token>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserValidation<IUser>, UserValidation>();
            #endregion
        }

    }

}