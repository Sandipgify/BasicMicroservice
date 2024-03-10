namespace Order.Infrastructure.Repository
{
    public class OrderRepository : Repository<Domain.Entity.Order>, IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entity.Order> GetById(long Id)
        {
            return await _dbContext.Order.Where(x=>x.IsActive && x.Id == Id).Include(x => x.OrderItems).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Domain.Entity.Order>> GetAll()
        {
            return await _dbContext.Order.Where(x => x.IsActive).Include(x => x.OrderItems).AsNoTracking().ToListAsync();
        }
    }
}
