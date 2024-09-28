using Application.Interfaces.Persistence;
using Domain.Entities;
using LanguageExt.Common;

namespace Infrastructure.Persistence.EntityFramework;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : AppDbContext, new()
{
    private readonly TContext _context;

    public bool IsDisposed { get; private set; }

    public UnitOfWork(TContext context)
    {
        _context = context;
        IsDisposed = false;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        return new Repository<T>(_context);
    }

    public Result<int> SaveChanges()
    {
        try
        {
            return _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context?.Dispose();
            IsDisposed = true;
        }
    }
}
