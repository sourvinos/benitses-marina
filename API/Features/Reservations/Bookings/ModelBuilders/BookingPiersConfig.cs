using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Bookings {

    internal class BookingPiersConfig : IEntityTypeConfiguration<BookingPier> {

        public void Configure(EntityTypeBuilder<BookingPier> entity) {
            // PK
            entity.Property(x => x.BookingId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // FKs
            entity.Property(x => x.Description).HasMaxLength(5).IsRequired(true);
        }

    }

}