using Domain.Entities;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IBaseService<Tentity, Tdto>
        where Tentity : BaseEntity
        where Tdto : class
    {

        #region Query

        ValueTask<Result<Tdto>> GetByIdAsync(Guid id, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default);
        ValueTask<Result<Tdto>> GetByIdAsync(Guid id, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties);
        ValueTask<Result<Tdto>> FirstOrDefaultAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties);
        ValueTask<Result<IEnumerable<Tdto>>> FindAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties);
        ValueTask<Result<IEnumerable<Tdto>>> GetAllAsync(bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties);
        ValueTask<Result<IEnumerable<Tdto>>> GetPaginatedAsync(PaginationParams paginationParams, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties);
        ValueTask<Result<long>> GetLongCountAsync(CancellationToken cancellationToken = default);
        #endregion

        #region Command

        ValueTask<Result<Tdto>> CreateAsync(Tdto dto, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        ValueTask<Result<Unit>> CreateRangeAsync(IEnumerable<Tdto> dtos, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, CancellationToken cancellationToken = default);

        ValueTask<Result<Tdto>> UpdateAsync(Tdto dto, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Unit>> UpdateRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate, Expression<Func<Tdto, Tdto>> updateDTOFactory, bool autoSave = true, CancellationToken cancellationToken = default);
        ValueTask<Result<Unit>> UpdateRangeAsync(IEnumerable<Tdto> dtos, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, CancellationToken cancellationToken = default);

        ValueTask<Result<Tdto>> RecoverAsync(Tdto dto, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> RecoverByIdAsync(Guid id, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> RecoverFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RecoverRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default);

        ValueTask<Result<Tdto>> SoftDeleteAsync(Tdto dto, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> SoftDeleteByIdAsync(Guid id, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> SoftDeleteFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Unit>> SoftDeleteRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default);

        ValueTask<Result<Tdto>> HardDeleteAsync(Tdto dto, bool autoSave = true, Func<Tdto, Tentity>? dtoToEntityMapper = null, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> HardDeleteByIdAsync(Guid id, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Tdto>> HardDeleteFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, Func<Tentity, Tdto>? entityToDTOMapper = null, CancellationToken cancellationToken = default);
        Task<Result<Unit>> HardDeleteRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool autoSave = true, CancellationToken cancellationToken = default);

        #endregion
    }
}
