using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.DocumentTypes {

    internal class DocumentTypeConfig : IEntityTypeConfiguration<SaleDocumentType> {

        public void Configure(EntityTypeBuilder<SaleDocumentType> entity) {
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Abbreviation).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.AbbreviationEn).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Batch).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.BatchEn).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.DiscriminatorId).IsRequired(true);
            entity.Property(x => x.IsActive);
            entity.Property(x => x.Customers).HasMaxLength(1).IsRequired(true);
            entity.Property(x => x.Suppliers).HasMaxLength(1);
            entity.Property(x => x.IsMyData);
            entity.Property(x => x.Table8_1).HasMaxLength(32);
            entity.Property(x => x.Table8_8).HasMaxLength(32);
            entity.Property(x => x.Table8_9).HasMaxLength(32);
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}