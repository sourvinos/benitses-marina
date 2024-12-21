using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Expenses.Expenses {

    internal class ExpensesConfig : IEntityTypeConfiguration<Expense> {

        public void Configure(EntityTypeBuilder<Expense> entity) {
            // PK
            entity.Property(x => x.Id).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // Fields
            entity.Property(x => x.Date).HasColumnType("date").IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}