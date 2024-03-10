using System.Linq.Expressions;

namespace Order.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly OrderDbContext _dbContext;
        protected Repository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T t)
        {
            await _dbContext.Set<T>().AddAsync(t);
            return t;
        }
        public void Update(T t) => _dbContext.Update(t);
        public async Task AddRangeAsync(IEnumerable<T> t) => await _dbContext.AddRangeAsync(t);
        public void UpdateRange(IEnumerable<T> t) => _dbContext.UpdateRange(t);
        public async Task<bool> Exist(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().AnyAsync(predicate);
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await (predicate is not null ? query.Where(predicate) : query).AsNoTracking().ToListAsync();
        }
        public async Task<T> GetById(object id) => await _dbContext.Set<T>().FindAsync(id);

    }
}
