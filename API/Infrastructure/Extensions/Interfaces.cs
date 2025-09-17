using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;
using API.Features.InsurancePolicies;
using API.Features.BoatTypes;
using API.Features.BoatUsages;
using API.Features.Leases;
using API.Features.Expenses.Banks;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.Ledgers;
using API.Features.Expenses.Companies;
using API.Features.Expenses.BalanceSheet;
using API.Features.Sales.Prices;
using API.Features.Expenses.Statistics;
using API.Features.Expenses.Transactions;
using API.Features.Reservations.Transactions;
using API.Features.Common.PaymentMethods;
using API.Features.Sales.Customers;
using API.Features.Sales.TaxOffices;
using API.Features.Sales.Nationalities;
using API.Features.Sales.DocumentTypes;
using API.Features.Sales.Invoices;
using API.Features.Cashiers.Transactions;
using API.Features.Cashiers.Ledgers;
using API.Features.Cashiers.Safes;
using API.Featuers.Sales.SeasonTypes;
using API.Featuers.Sales.HullTypes;
using API.Featuers.Sales.PeriodTypes;
using API.Infrastructure.Account;
using API.Features.Reservations.Parameters;
using API.Infrastructure.EmailServices;

namespace API.Infrastructure.Extensions {

    public static class Interfaces {

        public static void AddInterfaces(IServiceCollection services) {
            #region expenses
            services.AddTransient<IBalanceSheetRepository, BalanceSheetRepository>();
            services.AddTransient<IBankRepository, BankRepository>();
            services.AddTransient<IBankValidation, BankValidation>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyValidation, CompanyValidation>();
            services.AddTransient<IExpenseDocumentTypeRepository, ExpenseDocumentTypeRepository>();
            services.AddTransient<IExpenseDocumentTypeValidation, ExpenseDocumentTypeValidation>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IExpenseValidation, ExpenseValidation>();
            services.AddTransient<ILedgerRepository, LedgerRepository>();
            services.AddTransient<IPaymentStatusValidation, PaymentStatusValidation>();
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ISupplierValidation, SupplierValidation>();
            #endregion
            #region reservations
            services.AddTransient<IBerthRepository, BerthRepository>();
            services.AddTransient<IBerthValidation, BerthValidation>();
            services.AddTransient<IBoatTypeRepository, BoatTypeRepository>();
            services.AddTransient<IBoatTypeValidation, BoatTypeValidation>();
            services.AddTransient<IBoatUsageRepository, BoatUsageRepository>();
            services.AddTransient<IBoatUsageValidation, BoatUsageValidation>();
            services.AddTransient<IEndOfLeaseEmailSender, EndOfLeaseEmailSender>();
            services.AddTransient<IInsurancePolicyRepository, InsurancePolicyRepository>();
            services.AddTransient<IInvalidInsuranceEmailSender, InvalidInsuranceEmailSender>();
            services.AddTransient<ILeasePdfRepository, LeasePdfRepository>();
            services.AddTransient<ILeaseRepository, LeaseRepository>();
            services.AddTransient<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddTransient<IReservationEmailSender, ReservationEmailSender>();
            services.AddTransient<IReservationParametersRepository, ParametersRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IReservationValidation, ReservationValidation>();
            #endregion
            #region sales
            services.AddTransient<ICustomerAadeRepository, CustomerAadeRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerValidation, CustomerValidation>();
            services.AddTransient<IHullTypeRepository, HullTypeRepository>();
            services.AddTransient<IHullTypeValidation, HullTypeValidator>();
            services.AddTransient<IInvoiceCreateRepository, InvoiceCreateRepository>();
            services.AddTransient<IInvoiceDataUpRepository, InvoiceDataUpRepository>();
            services.AddTransient<IInvoiceReadRepository, InvoiceReadRepository>();
            services.AddTransient<IInvoiceValidation, InvoiceValidation>();
            services.AddTransient<INationalityRepository, NationalityRepository>();
            services.AddTransient<IPeriodTypeRepository, PeriodTypeRepository>();
            services.AddTransient<IPeriodTypeValidation, PeriodTypeValidator>();
            services.AddTransient<IPriceRepository, PriceRepository>();
            services.AddTransient<IPriceValidation, PriceValidation>();
            services.AddTransient<ISaleDocumentTypeRepository, SaleDocumentTypeRepository>();
            services.AddTransient<ISaleDocumentTypeValidation, SaleDocumentTypeValidation>();
            services.AddTransient<ISeasonTypeRepository, SeasonTypeRepository>();
            services.AddTransient<ISeasonTypeValidation, SeasonTypeValidator>();
            services.AddTransient<ITaxOfficeRepository, TaxOfficeRepository>();
            services.AddTransient<ITaxOfficeValidation, TaxOfficeValidator>();
            #endregion
            #region common
            services.AddScoped<Token>();
            services.AddTransient<IEmailAccountSender, EmailAccountSender>();
            services.AddTransient<IEmailQueueRepository, EmailQueueRepository>();
            services.AddTransient<IEmailUserDetailsSender, EmailUserDetailsSender>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IPaymentMethodValidation, PaymentMethodValidation>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserValidation<IUser>, UserValidation>();
            #endregion
            #region cashiers
            services.AddTransient<ICashierLedgerRepository, CashierLedgerRepository>();
            services.AddTransient<ICashierRepository, CashierRepository>();
            services.AddTransient<ICashierValidation, CashierValidation>();
            services.AddTransient<ISafeRepository, SafeRepository>();
            services.AddTransient<ISafeValidation, SafeValidation>();
            #endregion
        }

    }

}