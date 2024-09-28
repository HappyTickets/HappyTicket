using Domain.Entities;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.General;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Application.Interfaces.Persistence;

public interface IRepository<Tentity> where Tentity : BaseEntity
{
    #region Utilities

    IQueryable<Tentity> Query();
    void Attach(Tentity entity);
    void AttachRange(IEnumerable<Tentity> entities);
    void ModifyInsertState(Tentity entity);
    void ModifyUpdateState(Tentity entity);
    void ModifyUnchangedState(Tentity entity);
    void ModifyDeleteState(Tentity entity);
    void ModifyDeleteRangeState(IEnumerable<Tentity> entities);
    void Dispose();
    void ClearTrack();

    #endregion

    #region Query

    ValueTask<Result<Tentity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Tentity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<Tentity>> FirstOrDefaultAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<Tentity>> LastOrDefaultAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<IEnumerable<Tentity>>> FindAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<IEnumerable<Tentity>>> GetAllAsync(CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<IEnumerable<Tentity>>> GetPaginatedAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>[] includeProperties);
    Task<Result<long>> GetLongCountAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Command

    Task<Result<Tentity>> CreateAsync(Tentity entity, CancellationToken cancellationToken = default);
    Task<Result<Unit>> CreateRangeAsync(IEnumerable<Tentity> entities, CancellationToken cancellationToken = default);

    Result<Tentity> Update(Tentity entity);
    Result<Unit> UpdateRange(IEnumerable<Tentity> entities, CancellationToken cancellationToken = default);
    Task<Result<Unit>> UpdateRangeAsync(Expression<Func<Tentity, bool>> predicate, Func<Tentity, Tentity> updateFunc, CancellationToken cancellationToken = default);
    Task<Result<Unit>> UpdateRangeFactoryAsync(Expression<Func<Tentity, bool>> predicate, Expression<Func<Tentity, Tentity>> updateFactory, CancellationToken cancellationToken = default);

    Result<Tentity> Recover(Tentity entity);
    Task<Result<Tentity>> RecoverByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Tentity>> RecoverFirstAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<Result<Unit>> RecoverRangeAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default);

    Result<Tentity> SoftDelete(Tentity entity);
    Task<Result<Tentity>> SoftDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Tentity>> SoftDeleteFirstAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<Result<Unit>> SoftDeleteRangeAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default);

    Result<Tentity> HardDelete(Tentity entity);
    Task<Result<Tentity>> HardDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Tentity>> HardDeleteFirstAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken cancellationToken = default);
    Result<Unit> HardDeleteRange(Expression<Func<Tentity, bool>> predicate);


    #endregion
}
