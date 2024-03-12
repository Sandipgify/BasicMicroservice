using Product.Domain.Entity;

namespace Product.Application.Infrastructure
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> CategoryNameExist(string Name, long? id = null);
    }
}
