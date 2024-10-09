using Application.Interfaces.Persistence;
using Domain.Entities;
using Domain.Enums;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.General;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.EntityFramework;

public class Repository<Tentity> : IRepository<Tentity> where Tentity : BaseEntity
{
    protected readonly AppDbContext dbContext;
    protected readonly DbSet<Tentity> dbSet;
    public Repository(AppDbContext context)
    {
        dbContext = context ?? throw new ArgumentException(nameof(context));
        dbSet = dbContext.Set<Tentity>();
    }


    #region Utilities

    public IQueryable<Tentity> Query()
    {
        return dbSet.AsQueryable();
    }
    public void Attach(Tentity entity)
    {
        dbSet.Attach(entity);
    }
    public void AttachRange(IEnumerable<Tentity> entities)
    {
        dbSet.AttachRange(entities);
    }
    public void ModifyInsertState(Tentity entity)
    {
        dbSet.Entry(entity).State = EntityState.Added;
    }
    public void ModifyUpdateState(Tentity entity)
    {
        dbSet.Entry(entity).State = EntityState.Modified;
    }
    public void ModifyUnchangedState(Tentity entity)
    {
        dbSet.Entry(entity).State = EntityState.Unchanged;
    }
    public void ModifyDeleteState(Tentity entity)
    {
        dbSet.Entry(entity).State = EntityState.Deleted;
    }
    public void ModifyDeleteRangeState(IEnumerable<Tentity> entities)
    {
        foreach (Tentity entity in entities)
        {
            dbSet.Entry(entity).State = EntityState.Unchanged;
        }
    }
    public void ClearTrack()
    {
        dbContext.ChangeTracker.Clear();
    }
    public void Dispose()
    {
        dbContext.Dispose();
    }

    #endregion

    #region Query

    public async ValueTask<Result<Tentity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbSet.FindAsync(id, cancellationToken) ?? new Result<Tentity>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<Tentity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default,
                                                    params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;
            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            return await dbQuery.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? new Result<Tentity>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<Tentity>> FirstOrDefaultAsync(Expression<Func<Tentity, bool>> predicate,
                                                           CancellationToken cancellationToken = default,
                                                           params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;
            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            return await dbQuery.FirstOrDefaultAsync(predicate, cancellationToken) ?? new Result<Tentity>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<Tentity>> LastOrDefaultAsync(Expression<Func<Tentity, bool>> predicate,
                                                           CancellationToken cancellationToken = default,
                                                           params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;
            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            return await dbQuery.LastOrDefaultAsync(predicate, cancellationToken) ?? new Result<Tentity>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<IEnumerable<Tentity>>> FindAsync(Expression<Func<Tentity, bool>> where,
                                                              CancellationToken cancellationToken = default,
                                                              params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;
            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            return await dbQuery.Where(where).ToListAsync(cancellationToken) ?? new Result<IEnumerable<Tentity>>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<IEnumerable<Tentity>>> GetAllAsync(CancellationToken cancellationToken = default,
                                                                params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;
            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            List<Tentity> entities = await dbQuery.ToListAsync(cancellationToken);
            return entities.Any() ? new Result<IEnumerable<Tentity>>(entities) : new Result<IEnumerable<Tentity>>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }


    public async Task<Result<IEnumerable<Tentity>>> GetPaginatedAsync(PaginationParams paginationParams,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;

            if (includeProperties.Length != 0)
            {
                dbQuery = includeProperties.Aggregate(dbQuery, (oldQuery, includeProperty) => includeProperty.Compile().Invoke(oldQuery));
            }

            List<Tentity> entities = await dbQuery.Skip((paginationParams.PageIndex - 0) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return entities.Any() ? new Result<IEnumerable<Tentity>>(entities) : new Result<IEnumerable<Tentity>>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<Result<long>> GetLongCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<Tentity> dbQuery = dbSet;

            var count = await dbQuery.LongCountAsync(cancellationToken);

            return count > 0 ? new Result<long>(count) : new Result<long>(new NotFoundException());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    #endregion

    #region Command

    public async Task<Result<Tentity>> CreateAsync(Tentity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            return (await dbSet.AddAsync(entity, cancellationToken)).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Unit>> CreateRangeAsync(IEnumerable<Tentity> entities,
                                                     CancellationToken cancellationToken = default)
    {
        try
        {
            if (entities.Any())
            {
                await dbSet.AddRangeAsync(entities, cancellationToken);
            }
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public Result<Tentity> Update(Tentity entity)
    {
        try
        {
            Attach(entity);
            return dbSet.Update(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public Result<Unit> UpdateRange(IEnumerable<Tentity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entities.Any())
            {
                dbSet.UpdateRange(entities);
            }
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Unit>> UpdateRangeAsync(Expression<Func<Tentity, bool>> predicate,
                                                     Func<Tentity, Tentity> updateFunc,
                                                     CancellationToken cancellationToken = default)
    {
        try
        {
            List<Tentity> entities = await dbSet.Where(predicate).ToListAsync(cancellationToken);
            if (entities.Count != 0)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i] = updateFunc(entities[i]);
                }
                dbSet.UpdateRange(entities);
            }
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }
    public async Task<Result<Unit>> UpdateRangeFactoryAsync(Expression<Func<Tentity, bool>> predicate,
                                                     Expression<Func<Tentity, Tentity>> updateFactory,
                                                     CancellationToken cancellationToken = default)
    {
        try
        {
            var query = dbSet.Where(predicate);
            var entities = await query.Select(updateFactory).ToListAsync(cancellationToken);
            if (entities.Count != 0)
            {
                dbSet.UpdateRange(entities);
            }
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public Result<Tentity> Recover(Tentity entity)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            entity.BaseEntityStatus = BaseEntityStatus.Restored;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> RecoverByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            Tentity? entity = await dbSet.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = BaseEntityStatus.Restored;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> RecoverFirstAsync(Expression<Func<Tentity, bool>> predicate,
                                                         CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            Tentity? entity = await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = BaseEntityStatus.Restored;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Unit>> RecoverRangeAsync(Expression<Func<Tentity, bool>> predicate,
                                                      CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            var entities = dbSet.Where(predicate);
            await entities.ForEachAsync(entity => entity.BaseEntityStatus = BaseEntityStatus.Restored, cancellationToken);
            dbSet.RemoveRange(entities);
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public Result<Tentity> SoftDelete(Tentity entity)
    {
        try
        {
            entity.BaseEntityStatus = BaseEntityStatus.Archived;
            Tentity deletedEntity = dbSet.Remove(entity).Entity;
            return new Result<Tentity>(deletedEntity);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> SoftDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Tentity? entity = await dbSet.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = BaseEntityStatus.Archived;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> SoftDeleteFirstAsync(Expression<Func<Tentity, bool>> predicate,
                                                            CancellationToken cancellationToken = default)
    {
        try
        {
            Tentity? entity = await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = BaseEntityStatus.Archived;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Unit>> SoftDeleteRangeAsync(Expression<Func<Tentity, bool>> predicate,
                                                         CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = dbSet.Where(predicate);
            await entities.ForEachAsync(entity => entity.BaseEntityStatus = BaseEntityStatus.Archived);
            dbSet.RemoveRange(entities);
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public Result<Tentity> HardDelete(Tentity entity)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            entity.BaseEntityStatus = null;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> HardDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            Tentity? entity = await dbSet.FindAsync(id, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = null;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public async Task<Result<Tentity>> HardDeleteFirstAsync(Expression<Func<Tentity, bool>> predicate,
                                                            CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            Tentity? entity = await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            if (entity == null)
            {
                return new(new NotFoundException());
            }

            entity.BaseEntityStatus = null;
            return dbSet.Remove(entity).Entity;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public Result<Unit> HardDeleteRange(Expression<Func<Tentity, bool>> predicate)
    {
        try
        {
            dbContext.SwitchSoftDeleteFilter(true);
            dbSet.RemoveRange(dbSet.Where(predicate));
            return new(new Unit());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    #endregion
}
