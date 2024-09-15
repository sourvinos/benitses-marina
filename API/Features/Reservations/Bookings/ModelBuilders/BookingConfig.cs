using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Bookings {

    internal class BookingsConfig : IEntityTypeConfiguration<Booking> {

        public void Configure(EntityTypeBuilder<Booking> entity) {
            // PK
            entity.Property(x => x.BookingId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // FKs
            entity.Property(x => x.BoatTypeId).IsRequired(true);
            // Fields
            entity.Property(x => x.FromDate).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.Email).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.Remarks).HasDefaultValue("").HasMaxLength(128);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}