using Domain.Entities.Common;

namespace Application.Interfaces.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITicketRepository Tickets { get; }

    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<long>;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
