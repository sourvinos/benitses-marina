using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Invoices {

    internal class InvoiceAadeConfig : IEntityTypeConfiguration<InvoiceAade> {

        public void Configure(EntityTypeBuilder<InvoiceAade> entity) {
            entity.HasKey("InvoiceId");
            entity.Property(x => x.InvoiceId).IsFixedLength().HasMaxLength(36).IsRequired(true);
        }

    }

}