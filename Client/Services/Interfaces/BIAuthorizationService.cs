
using LanguageExt;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Client.Services.Interfaces
{
    public interface BIAuthorizationService
    {
        Task<Result<BaseResponse<Unit>>> AddRoleAsync(AddRoleDto addRoleDto);

        //Task<bool> IsRoleExistById(string roleId);

        //Task<bool> IsRoleExist(string roleName);

        Task<Result<BaseResponse<Unit>>> EditRoleAsync(EditRoleDto request);

        Task<Result<BaseResponse<Unit>>> DeleteRoleAsync(string roleId);

        Task<Result<BaseResponse<List<RoleDto>>>> GetRolesList();

        Task<Result<BaseResponse<RoleDto>>> GetRoleById(string id);

        Task<Result<BaseResponse<Unit>>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto);
        Task<Result<BaseResponse<Unit>>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto);

        Task<Result<BaseResponse<Unit>>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUserFromRoleDto);

        Task<Result<BaseResponse<UserWithRolesDto>>> GetUserWithRolesAsync(string userId);
        Task<Result<BaseResponse<List<UserWithRolesDto>>>> GetUsersWithRolesAsync();

        Task<Result<BaseResponse<RoleWithUsersDto>>> GetRoleWithUsersAsync(string roleId);
    }
}

