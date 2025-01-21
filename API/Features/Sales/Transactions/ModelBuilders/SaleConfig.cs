using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Transactions {

    internal class InvoicesConfig : IEntityTypeConfiguration<Sale> {

        public void Configure(EntityTypeBuilder<Sale> entity) {
            entity.Property(x => x.InvoiceId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            entity.Property(x => x.CustomerId).IsRequired(true);
            entity.Property(x => x.DocumentTypeId).IsRequired(true);
            entity.Property(x => x.Date).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.TripDate).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}