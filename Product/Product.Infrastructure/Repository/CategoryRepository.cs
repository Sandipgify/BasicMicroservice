namespace Product.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
