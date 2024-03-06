using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Product.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly ProductDbContext _dbContext;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }
        public void Save() => _dbContext.SaveChanges();
        public async Task SaveAsync(CancellationToken cancellationtoken = default) => await _dbContext.SaveChangesAsync(cancellationtoken);
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public void Rollback() => _dbContext.Database.RollbackTransaction();
        public async Task RollbackAsync() => await _dbContext.Database.RollbackTransactionAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
