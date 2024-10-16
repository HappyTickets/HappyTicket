using Domain.Entities.Common;

namespace Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<long>;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
