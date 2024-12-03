using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Reservations;
using API.Features.Reservations.PaymentStatuses;
using API.Features.BoatTypes;
using API.Features.BoatUsages;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new BerthsConfig());
            modelBuilder.ApplyConfiguration(new BoatTypesConfig());
            modelBuilder.ApplyConfiguration(new PaymentStatusesConfig());
            modelBuilder.ApplyConfiguration(new ReservationBerthsConfig());
            modelBuilder.ApplyConfiguration(new ReservationsConfig());
            modelBuilder.ApplyConfiguration(new UsersConfig());
        }

    }

}