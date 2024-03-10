namespace Order.Application.Infrastructure
{
    public interface IOrderRepository : IRepository<Domain.Entity.Order>
    {
        Task<Domain.Entity.Order> GetById(long Id);
        Task<IEnumerable<Domain.Entity.Order>> GetAll();
    }
}
