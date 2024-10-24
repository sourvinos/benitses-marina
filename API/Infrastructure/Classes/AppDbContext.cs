using API.Features.Reservations.Berths;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Reservations;
using API.Features.Reservations.PaymentStatuses;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Berth> Berths { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationBerth> ReservationBerths { get; set; }
        public DbSet<ReservationLease> ReservationLease { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new BerthsConfig());
            modelBuilder.ApplyConfiguration(new PaymentStatusesConfig());
            modelBuilder.ApplyConfiguration(new ReservationBerthsConfig());
            modelBuilder.ApplyConfiguration(new ReservationsConfig());
            modelBuilder.ApplyConfiguration(new UsersConfig());
        }

    }

}