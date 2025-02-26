using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.DocumentTypes {

    internal class SaleDocumentTypeConfig : IEntityTypeConfiguration<SaleDocumentType> {

        public void Configure(EntityTypeBuilder<SaleDocumentType> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // Fields
            entity.Property(x => x.DiscriminatorId).IsRequired(true);
            entity.Property(x => x.Abbreviation).HasMaxLength(16).IsRequired(true);
            entity.Property(x => x.AbbreviationEn).HasMaxLength(16).IsRequired(true);
            entity.Property(x => x.AbbreviationDataUp).HasMaxLength(16).IsRequired(true);
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Batch).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.Customers).HasMaxLength(1).IsRequired(true);
            entity.Property(x => x.IsStatistic);
            entity.Property(x => x.IsDefault);
            entity.Property(x => x.IsActive);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}