using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Cashiers.Transactions {

    internal class CashierConfig : IEntityTypeConfiguration<Cashier> {

        public void Configure(EntityTypeBuilder<Cashier> entity) {
            // PK
            entity.Property(x => x.CashierId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // Fields
            entity.Property(x => x.Date).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.CompanyId).IsRequired(true);
            entity.Property(x => x.SafeId).IsRequired(true);
            entity.Property(x => x.Amount).IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}