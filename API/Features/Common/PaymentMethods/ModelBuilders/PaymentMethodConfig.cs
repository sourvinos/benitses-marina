using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Common.PaymentMethods {

    internal class PaymentMethodsConfig : IEntityTypeConfiguration<PaymentMethod> {

        public void Configure(EntityTypeBuilder<PaymentMethod> entity) {
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.IsActive);
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}