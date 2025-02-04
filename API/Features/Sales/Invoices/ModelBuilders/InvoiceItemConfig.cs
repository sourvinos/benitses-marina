using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Invoices {

    internal class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItem> {

        public void Configure(EntityTypeBuilder<InvoiceItem> entity) {
            entity.Property(x => x.InvoiceId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            entity.Property(x => x.Code).HasMaxLength(10).IsRequired(true);
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.EnglishDescription).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Remarks).HasMaxLength(512);
            entity.Property(x => x.Quantity).IsRequired();
            entity.Property(x => x.NetAmount).IsRequired();
            entity.Property(x => x.VatPercent).IsRequired();
            entity.Property(x => x.VatAmount).IsRequired();
            entity.Property(x => x.GrossAmount).IsRequired();
        }

    }

}