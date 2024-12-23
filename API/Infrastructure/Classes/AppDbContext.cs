using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Reservations;
using API.Features.BoatTypes;
using API.Features.BoatUsages;
using API.Features.Expenses.Banks;
using API.Features.Expenses.PaymentMethods;
using API.Features.Expenses.Suppliers;
using API.Features.Reservations.PaymentStatuses;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.Invoices;
using API.Features.Invoices.Invoices;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region Expenses

        public DbSet<Bank> Banks { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        #endregion

        #region Reservations
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
        public DbSet<Token> Tokens { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            #region Expenses
            modelBuilder.ApplyConfiguration(new BanksConfig());
            modelBuilder.ApplyConfiguration(new DocumentTypesConfig());
            modelBuilder.ApplyConfiguration(new InvoicesConfig());
            modelBuilder.ApplyConfiguration(new PaymentMethodsConfig());
            modelBuilder.ApplyConfiguration(new SuppliersConfig());
            #endregion
            #region Reservations
            modelBuilder.ApplyConfiguration(new BerthsConfig());
            modelBuilder.ApplyConfiguration(new BoatTypesConfig());
            modelBuilder.ApplyConfiguration(new BoatUsagesConfig());
            modelBuilder.ApplyConfiguration(new ReservationBerthsConfig());
            modelBuilder.ApplyConfiguration(new ReservationsConfig());
            modelBuilder.ApplyConfiguration(new UsersConfig());
            #endregion

        }

    }

}