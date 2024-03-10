using System.Linq.Expressions;

namespace Order.Application.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T t);
        void Update(T t);
        Task AddRangeAsync(IEnumerable<T> t);
        void UpdateRange(IEnumerable<T> t);
        Task<bool> Exist(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetById(object id);
    }
}
