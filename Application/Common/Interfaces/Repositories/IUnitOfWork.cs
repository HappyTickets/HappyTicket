using Domain.Entities.Common;

namespace Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    ITicketRepository Tickets { get; }

    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<long>;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
