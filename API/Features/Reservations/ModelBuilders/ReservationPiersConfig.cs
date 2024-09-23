using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations {

    internal class ReservationPiersConfig : IEntityTypeConfiguration<ReservationPier> {

        public void Configure(EntityTypeBuilder<ReservationPier> entity) {
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            entity.Property(x => x.PierId).IsRequired(true);
        }

    }

}