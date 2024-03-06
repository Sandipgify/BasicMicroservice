
using System.Linq.Expressions;

namespace Product.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ProductDbContext _dbContext;
        protected Repository(ProductDbContext dbContext)
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

    }
}
