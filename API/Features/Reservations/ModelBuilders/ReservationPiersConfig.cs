using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations {

    internal class ReservationPiersConfig : IEntityTypeConfiguration<ReservationPier> {

        public void Configure(EntityTypeBuilder<ReservationPier> entity) {
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            entity.Property(x => x.Description).HasMaxLength(5).IsRequired(true);
        }

    }

}