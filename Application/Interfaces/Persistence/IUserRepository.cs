using Domain.Entities.UserEntities;
using LanguageExt.Common;
using System.Linq.Expressions;

namespace Application.Interfaces.Persistence;

public interface IUserRepository<TUser> where TUser : ApplicationUser
{
    public Task<Result<TUser>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<TUser>> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
    public Task<Result<TUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<Result<IEnumerable<TUser>>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Result<IEnumerable<TUser>>> FindAsync(Expression<Func<TUser, bool>> predicate, CancellationToken cancellationToken = default);

    public Task<Result<TUser>> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default);

    public Task<Result<TUser>> UpdateAsync(TUser user, CancellationToken cancellationToken = default);

    public Task<Result<TUser>> RecoverAsync(string Id, CancellationToken cancellationToken = default);
    public Task<Result<TUser>> SoftDeleteAsync(string Id, CancellationToken cancellationToken = default);
    public Task<Result<TUser>> HardDeleteAsync(string Id, CancellationToken cancellationToken = default);
}
