using API.Features.Reservations.Piers;
using API.Infrastructure.Auth;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Reservations.Bookings;

namespace API.Infrastructure.Classes {

    public class AppDbContext : IdentityDbContext<IdentityUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPier> BookingPiers { get; set; }
        public DbSet<Pier> Piers { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        private static void ApplyConfigurations(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new BookingsConfig());
            modelBuilder.ApplyConfiguration(new BookingPiersConfig());
            modelBuilder.ApplyConfiguration(new PiersConfig());
            modelBuilder.ApplyConfiguration(new UsersConfig());
        }

    }

}