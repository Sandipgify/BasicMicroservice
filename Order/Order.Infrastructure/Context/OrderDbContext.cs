using Order.Infrastructure.EntityConfiguration;

namespace Order.Infrastructure.Context
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Domain.Entity.Order> Order { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new OrderEntityConfiguration())
                .ApplyConfiguration(new OrderItemEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
        }

    }
}
