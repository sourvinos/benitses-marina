using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Invoices {

    internal class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItem> {

        public void Configure(EntityTypeBuilder<InvoiceItem> entity) {
            entity.HasKey("InvoiceId");
            entity.Property(x => x.InvoiceId).IsFixedLength().HasMaxLength(36).IsRequired(true);
        }

    }

}