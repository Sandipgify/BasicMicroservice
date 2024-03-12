namespace Product.Infrastructure.Repository
{
    public class ProductRepository : Repository<Domain.Entity.Product>, IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ProductNameExist(string Name, long? id = null)
        {
            return await _dbContext.Set<Domain.Entity.Product>().AnyAsync(c => c.Name == Name && c.IsActive && (id == null || c.Id == id));
        }
    }
}
