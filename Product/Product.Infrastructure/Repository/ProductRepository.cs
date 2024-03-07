namespace Product.Infrastructure.Repository
{
    public class ProductRepository : Repository<Domain.Entity.Product>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
