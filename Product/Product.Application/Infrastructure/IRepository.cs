using System.Linq.Expressions;

namespace Product.Application.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T t);
        void Update(T t);
        Task AddRangeAsync(IEnumerable<T> t);
        void UpdateRange(IEnumerable<T> t);
        Task<bool> Exist(Expression<Func<T, bool>> predicate);
    }
}
