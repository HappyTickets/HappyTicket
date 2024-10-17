﻿using Domain.Entities;
using Domain.Entities.Common;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Interfaces.Infrastructure.Persistence;

public interface IRepository<TEntity> where TEntity : BaseEntity<long>
{
    // commands
    void Create(TEntity entity);
    void CreateRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void HardDelete(TEntity entity);
    void HardDeleteRange(IEnumerable<TEntity> entities);
    void SoftDelete(SoftDeletableEntity<long> entity);
    void SoftDeleteRange(IEnumerable<SoftDeletableEntity<long>> entities);

    void Recover(SoftDeletableEntity<long> entity);

    void RecoverRange(IEnumerable<SoftDeletableEntity<long>> entities);

    // queries
    IQueryable<TEntity> Query();

    Task<TEntity?> GetByIdAsync(long id, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, bool ignoreFilter = false);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default);
    Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, bool ignoreFilter = false);

    Task<PaginatedList<TEntity>> PaginateAsync(int pageIndex, int pageSize, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default);
    Task<PaginatedList<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, IEnumerable<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<long> CountAsync(CancellationToken cancellationToken = default);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}