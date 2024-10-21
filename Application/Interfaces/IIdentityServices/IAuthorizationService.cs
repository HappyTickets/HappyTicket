using LanguageExt;
using LanguageExt.Common;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Application.Interfaces.IIdentityServices
{
    public interface IAuthorizationService
    {
        Task<Result<Unit>> AddRoleAsync(AddRoleDto addRoleDto);
        Task<bool> IsRoleExistById(string roleId);
        Task<bool> IsRoleExist(string roleName);
        Task<Result<Unit>> EditRoleAsync(EditRoleDto request);
        Task<Result<Unit>> DeleteRoleAsync(string roleId);
        Task<Result<List<RoleDto>>> GetRolesList(CancellationToken cancellationToken = default);
        Task<Result<RoleDto>> GetRoleById(string id);

        Task<Result<Unit>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto);
        Task<Result<Unit>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUsersFromRoleDto, CancellationToken cancellationToken = default);

        Task<Result<UserWithRolesDto>> GetUserWithRolesAsync(string userId);
        Task<Result<RoleWithUsersDto>> GetRoleWithUsersAsync(string roleId, PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default);

        Task<Result<PaginatedList<UserWithRolesDto>>> GetUsersWithRolesAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken);
    }

}