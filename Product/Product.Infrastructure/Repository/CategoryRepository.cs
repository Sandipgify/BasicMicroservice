namespace Product.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ProductDbContext _dbContext;

        public CategoryRepository(ProductDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CategoryNameExist(string Name, long? id = null)
        {
            return await _dbContext.Set<Category>().AnyAsync(c => c.Name == Name && c.IsActive && (id == null || c.Id == id));
        }
    }
}
