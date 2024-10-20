using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations {

    internal class ReservationBerthsConfig : IEntityTypeConfiguration<ReservationBerth> {

        public void Configure(EntityTypeBuilder<ReservationBerth> entity) {
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            entity.Property(x => x.Description).IsRequired(true);
        }

    }

}