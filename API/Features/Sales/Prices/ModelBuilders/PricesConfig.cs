using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Prices {

    internal class PricesConfig : IEntityTypeConfiguration<Price> {

        public void Configure(EntityTypeBuilder<Price> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // Fields
            entity.Property(x => x.Description).HasMaxLength(10).IsRequired(true);
            entity.Property(x => x.LongDescription).HasMaxLength(100).IsRequired(true);
            entity.Property(x => x.FromDate).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.ToDate).HasColumnType("date").IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}