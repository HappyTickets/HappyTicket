using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces.Persistence
{
    public interface ITransactionRepository : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }

}
