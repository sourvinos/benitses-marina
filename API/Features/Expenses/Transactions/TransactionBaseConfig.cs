using API.Features.Expenses.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Expenses.Transactions {

    internal class TransactionConfig : IEntityTypeConfiguration<TransactionsBase> {

        public void Configure(EntityTypeBuilder<TransactionsBase> entity) {
            entity.HasKey(x => x.Id);
            entity.HasDiscriminator<int>("DiscriminatorId")
                .HasValue<TransactionsBase>(0)
                .HasValue<Invoice>(1);
        }

    }

}