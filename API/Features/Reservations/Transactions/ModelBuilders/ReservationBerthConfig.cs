using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Transactions {

    internal class ReservationBerthConfig : IEntityTypeConfiguration<ReservationBerth> {

        public void Configure(EntityTypeBuilder<ReservationBerth> entity) {
            // PK
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // Fields
            entity.Property(x => x.Description).IsRequired(true);
        }

    }

}