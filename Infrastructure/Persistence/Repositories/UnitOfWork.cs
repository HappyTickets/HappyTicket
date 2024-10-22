using Application.Common.Interfaces.Persistence;
using Domain.Entities.Common;

namespace Infrastructure.Persistence.EntityFramework;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repos;

    public ITicketRepository Tickets { get; }
    public IOrderRepository Orders { get; }

    public UnitOfWork(AppDbContext context, ITicketRepository tickets, IOrderRepository orders)
    {
        _context = context;
        _repos = new();
        Tickets = tickets;
        Orders = orders;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<long>
    {
        if (_repos.ContainsKey(typeof(TEntity)))
            return (IRepository<TEntity>)_repos[typeof(TEntity)];

        var repo = new Repository<TEntity>(_context);
        _repos.Add(typeof(TEntity), repo);

        return repo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}
