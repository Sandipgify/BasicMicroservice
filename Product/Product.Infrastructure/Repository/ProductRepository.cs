namespace Product.Infrastructure.Repository
{
    public class ProductRepository : Repository<Domain.Entity.Product>, IProductRepository
    {
        protected ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
