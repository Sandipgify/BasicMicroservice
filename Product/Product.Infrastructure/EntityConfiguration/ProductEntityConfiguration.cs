using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.EntityConfiguration
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Domain.Entity.Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entity.Product> builder)
        {
            builder.Property(p => p.Name)
           .IsRequired()
           .HasMaxLength(255);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
