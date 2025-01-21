using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Transactions {

    internal class InvoicesAadeConfig : IEntityTypeConfiguration<SaleAade> {

        public void Configure(EntityTypeBuilder<SaleAade> entity) {
            entity.HasKey("InvoiceId");
        }

    }

}