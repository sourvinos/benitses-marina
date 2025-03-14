using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Transactions {

    internal class ReservationConfig : IEntityTypeConfiguration<Reservation> {

        public void Configure(EntityTypeBuilder<Reservation> entity) {
            // PK
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // Fields
            entity.Property(x => x.FromDate).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.ToDate).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.Remarks).HasDefaultValue("").HasMaxLength(2048);
            entity.Property(x => x.FinancialRemarks).HasDefaultValue("").HasMaxLength(2048);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}