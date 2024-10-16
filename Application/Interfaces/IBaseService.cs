using Domain.Entities.Common;
using LanguageExt;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity<long>
    {
        #region Query
        ValueTask<TDto?> GetByIdAsync<TDto>(long id, CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;
        ValueTask<TDto?> FirstOrDefaultAsync<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;
        ValueTask<IEnumerable<TDto>> GetAllAsync<TDto>(CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;


        ValueTask<IEnumerable<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;

        ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(PaginationSearchModel paginationParams, CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;
        ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(Expression<Func<TEntity, bool>> predicate, PaginationSearchModel paginationParams, CancellationToken cancellationToken = default, IEnumerable<Expression<Func<TEntity, object>>>? includes = null) where TDto : class;

        ValueTask<long> GetLongCountAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Command

        ValueTask<Unit> CreateAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;
        ValueTask<Unit> CreateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<Unit> UpdateAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;
        ValueTask<Unit> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<TDto?> RecoverAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> RecoverByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<Unit> RecoverRangeAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);

        ValueTask<Unit> SoftDeleteAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> SoftDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<Unit> SoftDeleteFirstAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<Unit> SoftDeleteRangeAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);

        ValueTask<Unit> HardDeleteAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> HardDeleteByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<Unit> HardDeleteFirstAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);
        Task<Unit> HardDeleteRangeAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = true, CancellationToken cancellationToken = default);

        #endregion
    }

}
