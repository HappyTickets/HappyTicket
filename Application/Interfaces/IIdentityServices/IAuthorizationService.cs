using LanguageExt;
using LanguageExt.Common;
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
        Task<Result<List<RoleDto>>> GetRolesList();
        Task<Result<RoleDto>> GetRoleById(string id);

        Task<Result<Unit>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto);
        Task<Result<Unit>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto);
        Task<Result<Unit>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUserFromRoleDto);

        Task<Result<UserWithRolesDto>> GetUserWithRolesAsync(string userId);
        Task<Result<RoleWithUsersDto>> GetRoleWithUsersAsync(string roleId);
        Task<Result<List<UserWithRolesDto>>> GetUsersWithRolesAsync();

    }
}
