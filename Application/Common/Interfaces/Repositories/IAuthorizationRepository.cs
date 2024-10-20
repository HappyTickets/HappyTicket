using Domain.Entities.UserEntities;
using LanguageExt;
using LanguageExt.Common;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Application.Interfaces.Persistence
{
    public interface IAuthorizationRepository
    {
        Task<Result<Unit>> UnassignUsersFromRoleAsync(RemoveUsersFromRoleDto dto, CancellationToken cancellationToken = default);
        IQueryable<ApplicationUser> GetUsersInRole(string roleId);

        IEnumerable<UserWithRolesDto> GetUsersWithRolesAsync(IQueryable<ApplicationUser> users, CancellationToken cancellationToken);
        Task<Result<Unit>> AssignUsersToRoleAsync(AssignUsersToRoleDto dto, CancellationToken cancellationToken = default);
    }
}