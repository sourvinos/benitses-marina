using API.Features.BoatTypes;
using API.Features.BoatUsages;
using API.Features.Reservations.Berths;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Sales.Prices;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Expenses.Transactions;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.Companies;
using API.Features.Expenses.Banks;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.Transactions;
using API.Features.Common.PaymentMethods;
using API.Features.Sales.Customers;
using API.Features.Sales.Nationalities;
using API.Features.Sales.TaxOffices;
using API.Features.Sales.DocumentTypes;
using API.Features.Sales.Invoices;
using API.Features.Sales.Transactions;
using API.Features.Cashiers.Transactions;
using API.Features.Cashiers.Safes;
using API.Featuers.Sales.SeasonTypes;
using API.Featuers.Sales.HullTypes;
using API.Featuers.Sales.PeriodTypes;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region expenses

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ExpenseDocumentType> ExpenseDocumentTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        #endregion

        #region reservations

        public DbSet<Berth> Berths { get; set; }
        public DbSet<BoatType> BoatTypes { get; set; }
        public DbSet<BoatUsage> BoatUsages { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationBerth> ReservationBerths { get; set; }
        public DbSet<ReservationBilling> ReservationBillingDetails { get; set; }
        public DbSet<ReservationBoat> ReservationBoats { get; set; }
        public DbSet<ReservationFee> ReservationFeeDetails { get; set; }
        public DbSet<ReservationInsurance> ReservationInsuranceDetails { get; set; }
        public DbSet<ReservationOwner> ReservationOwnerDetails { get; set; }

        #endregion

        #region common

        public DbSet<Token> Tokens { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        #endregion

        #region cashiers

        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Safe> Safes { get; set; }

        #endregion

        #region sales

        public DbSet<Customer> Customers { get; set; }
        public DbSet<HullType> HullTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoicesItems { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<PeriodType> PeriodTypes { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<SaleDocumentType> SaleDocumentTypes { get; set; }
        public DbSet<SeasonType> SeasonTypes { get; set; }
        public DbSet<TaxOffice> TaxOffices { get; set; }
        public DbSet<TransactionsBase> Sales { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            #region expenses
            modelBuilder.ApplyConfiguration(new BankConfig());
            modelBuilder.ApplyConfiguration(new ExpenseDocumentTypeConfig());
            modelBuilder.ApplyConfiguration(new ExpenseConfig());
            modelBuilder.ApplyConfiguration(new SupplierConfig());
            #endregion
            #region reservations
            modelBuilder.ApplyConfiguration(new BerthConfig());
            modelBuilder.ApplyConfiguration(new BoatTypeConfig());
            modelBuilder.ApplyConfiguration(new BoatUsageConfig());
            modelBuilder.ApplyConfiguration(new ReservationBerthConfig());
            modelBuilder.ApplyConfiguration(new ReservationConfig());
            #endregion
            #region common
            modelBuilder.ApplyConfiguration(new PaymentMethodConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            #endregion
            #region cashiers
            modelBuilder.ApplyConfiguration(new CashierConfig());
            #endregion
            #region sales
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new PriceConfig());
            modelBuilder.ApplyConfiguration(new SaleDocumentTypeConfig());
            modelBuilder.ApplyConfiguration(new TaxOfficeConfig());
            modelBuilder.ApplyConfiguration(new TransactionConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
            #endregion
        }

    }

}