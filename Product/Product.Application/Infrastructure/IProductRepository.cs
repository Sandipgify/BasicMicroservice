namespace Product.Application.Infrastructure
{
    public interface IProductRepository:IRepository<Domain.Entity.Product>
    {
        Task<bool> ProductNameExist(string Name, long? id = null);
    }
}
