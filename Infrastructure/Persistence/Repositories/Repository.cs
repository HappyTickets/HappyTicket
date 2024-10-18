using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Domain.Entities.Common;
using Infrastructure.Persistence.Extensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.EntityFramework;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<long>
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    public Repository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<TEntity>();
    }

    #region Command

    public void Create(TEntity entity)
        => _dbSet.Add(entity);

    public void CreateRange(IEnumerable<TEntity> entities)
        => _dbSet.AddRange(entities);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities)
        => _dbSet.UpdateRange(entities);

    public void HardDelete(TEntity entity)
        => _dbSet.Remove(entity);

    public void HardDeleteRange(IEnumerable<TEntity> entities)
        => _dbSet.RemoveRange(entities);

    public void SoftDelete(SoftDeletableEntity<long> entity)
    {
        entity.IsActive = false;
        entity.SoftDeleteCount += 1;

        _dbContext.Update(entity);
    }

    public void SoftDeleteRange(IEnumerable<SoftDeletableEntity<long>> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.SoftDeleteCount += 1;
        }

        _dbContext.Update(entities);
    }

    public void Recover(SoftDeletableEntity<long> entity)
    {
        entity.IsActive = true;
        _dbContext.Update(entity);
    }

    public void RecoverRange(IEnumerable<SoftDeletableEntity<long>> entities)
    {
        foreach (var entity in entities)
            entity.IsActive = true;

        _dbContext.UpdateRange(entities);
    }

    #endregion

    #region Query

    public IQueryable<TEntity> Query()
        => _dbSet.AsQueryable();

    public Task<TEntity?> GetByIdAsync(long id, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, bool ignoreFilter = false)
    {
        var query = _dbSet.AsQueryable();

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();

        if (includes != null)
            query = query.Include(includes);

        return query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (includes != null)
            query = query.Include(includes);

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (includes != null)
            query = query.Include(includes);

        return await query.LastOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (includes != null)
            query = query.Include(includes);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, bool ignoreFilter = false)
    {
        var query = _dbSet.Where(predicate).AsQueryable();

        if (includes != null)
            query = query.Include(includes);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<TEntity>> PaginateAsync(int pageIndex, int pageSize, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (includes != null)
            query = query.Include(includes);

        return await _dbSet.PaginateAsync(pageIndex, pageSize);
    }

    public async Task<PaginatedList<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate).AsQueryable();

        if (includes != null)
            query = query.Include(includes);

        return await _dbSet.PaginateAsync(pageIndex, pageSize);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(predicate, cancellationToken);

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(cancellationToken);

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
        => _dbSet.LongCountAsync(cancellationToken);

    public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet.LongCountAsync(predicate, cancellationToken);

    #endregion

}
