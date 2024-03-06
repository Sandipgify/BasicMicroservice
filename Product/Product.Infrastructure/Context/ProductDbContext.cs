using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.EntityConfiguration;

namespace Product.Infrastructure.Context
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProductEntityConfiguration())
                .ApplyConfiguration(new CategoryEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
        }

    }
}
