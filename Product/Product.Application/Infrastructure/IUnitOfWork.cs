namespace Product.Application.Infrastructure
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        void Save();
        Task SaveAsync(CancellationToken cancellationtoken = default);
        Task CommitTransactionAsync();
        void Rollback();
        Task RollbackAsync();

    }
}
