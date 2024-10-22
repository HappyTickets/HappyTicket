using Domain.Entities.UserEntities;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Application.Interfaces.Persistence
{
    public interface IAuthorizationRepository
    {
        Task<Empty> UnassignUsersFromRoleAsync(RemoveUsersFromRoleDto dto, CancellationToken cancellationToken = default);
        IQueryable<ApplicationUser> GetUsersInRole(long roleId);

        IEnumerable<UserWithRolesDto> GetUsersWithRolesAsync(IQueryable<ApplicationUser> users, CancellationToken cancellationToken);
        Task<Empty> AssignUsersToRoleAsync(AssignUsersToRoleDto dto, CancellationToken cancellationToken = default);
    }
}