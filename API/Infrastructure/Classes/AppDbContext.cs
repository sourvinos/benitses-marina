using API.Features.BoatTypes;
using API.Features.BoatUsages;
using API.Features.Reservations.Berths;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Sales.Customers;
using API.Features.Sales.Nationalities;
using API.Features.Sales.Prices;
using API.Features.Sales.TaxOffices;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Expenses.Transactions;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.Companies;
using API.Features.Expenses.Banks;
using API.Features.Expenses.BalanceFilters;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.Transactions;
using API.Features.Sales.Transactions;
using API.Features.Sales.DocumentTypes;
using API.Features.Common.PaymentMethods;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region expenses
        public DbSet<BalanceFilter> BalanceFilters { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
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

        #region sales
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SaleDocumentType> SaleDocumentTypes { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleAade> SalesAade { get; set; }
        public DbSet<TaxOffice> TaxOffices { get; set; }
        #endregion

        #region common
        public DbSet<Token> Tokens { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            #region expenses
            modelBuilder.ApplyConfiguration(new BanksConfig());
            modelBuilder.ApplyConfiguration(new DocumentTypesConfig());
            modelBuilder.ApplyConfiguration(new ExpenseConfig());
            modelBuilder.ApplyConfiguration(new SuppliersConfig());
            #endregion
            #region reservations
            modelBuilder.ApplyConfiguration(new BerthsConfig());
            modelBuilder.ApplyConfiguration(new BoatTypesConfig());
            modelBuilder.ApplyConfiguration(new BoatUsagesConfig());
            modelBuilder.ApplyConfiguration(new ReservationBerthsConfig());
            modelBuilder.ApplyConfiguration(new ReservationsConfig());
            #endregion
            #region common
            modelBuilder.ApplyConfiguration(new PaymentMethodsConfig());
            modelBuilder.ApplyConfiguration(new UsersConfig());
            #endregion
        }

    }

}