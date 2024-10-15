using Domain.Entities.Common;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity<long>
    {
        #region Query

        ValueTask<TDto?> GetByIdAsync<TDto>(long id, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        ValueTask<TDto?> GetByIdAsync<TDto>(long id, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;
        ValueTask<TDto?> FirstOrDefaultAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;
        ValueTask<IEnumerable<TDto>> GetAllAsync<TDto>(Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;

        ValueTask<IEnumerable<TDto>> FindAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;

        ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(PaginationSearchModel paginationParams, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;
        ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, PaginationSearchModel paginationParams, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class;

        ValueTask<long> GetLongCountAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Command

        ValueTask<Unit> CreateAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        ValueTask<Unit> CreateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<Unit> UpdateAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        ValueTask<Unit> UpdateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<TDto?> RecoverAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<TDto?> RecoverByIdAsync<TDto>(long id, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<TDto?> RecoverFirstAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> RecoverRangeAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<Unit> SoftDeleteAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> SoftDeleteByIdAsync<TDto>(long id, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> SoftDeleteFirstAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> SoftDeleteRangeAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;

        ValueTask<Unit> HardDeleteAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> HardDeleteByIdAsync<TDto>(long id, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> HardDeleteFirstAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class;
        Task<Unit> HardDeleteRangeAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class;

        #endregion
    }

}
