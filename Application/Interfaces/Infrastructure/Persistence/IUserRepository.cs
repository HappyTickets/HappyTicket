using Domain.Entities.UserEntities;
using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Interfaces.Infrastructure.Persistence;

public interface IUserRepository<TUser> where TUser : ApplicationUser
{
    public Task<BaseResponse<TUser>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<BaseResponse<TUser>> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
    public Task<BaseResponse<TUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<BaseResponse<IEnumerable<TUser>>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<BaseResponse<PaginatedList<TUser>>> GetAllAsync(PaginationSearchModel queryParams, CancellationToken cancellationToken = default);
    public Task<BaseResponse<IEnumerable<TUser>>> FindAsync(Expression<Func<TUser, bool>> predicate, CancellationToken cancellationToken = default);

    public Task<BaseResponse<TUser>> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default);

    public Task<BaseResponse<TUser>> UpdateAsync(TUser user, CancellationToken cancellationToken = default);

    public Task<BaseResponse<TUser>> RecoverAsync(string Id, CancellationToken cancellationToken = default);
    public Task<BaseResponse<TUser>> SoftDeleteAsync(string Id, CancellationToken cancellationToken = default);
    public Task<BaseResponse<TUser>> HardDeleteAsync(string Id, CancellationToken cancellationToken = default);
}
