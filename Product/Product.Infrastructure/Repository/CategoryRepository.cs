namespace Product.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        protected CategoryRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
