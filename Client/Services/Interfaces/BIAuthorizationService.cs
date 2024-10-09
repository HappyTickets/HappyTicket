
using LanguageExt;
using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Client.Services.Interfaces
{
    public interface BIAuthorizationService
    {
        Task<Result<BaseResponse<Unit>>> AddRoleAsync(AddRoleDto addRoleDto);
        //Task<BaseResponse<bool>> IsRoleExistById(string roleId);
        //Task<BaseResponse<bool>> IsRoleExist(string roleName);
        Task<Result<BaseResponse<Unit>>> EditRoleAsync(EditRoleDto request);
        Task<Result<BaseResponse<Unit>>> DeleteRoleAsync(string roleId);
        Task<Result<BaseResponse<List<RoleDto>>>> GetRolesList(CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<RoleDto>>> GetRoleById(string id);

        Task<Result<BaseResponse<Unit>>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto);
        Task<Result<BaseResponse<Unit>>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<Unit>>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUsersFromRoleDto, CancellationToken cancellationToken = default);

        Task<Result<BaseResponse<UserWithRolesDto>>> GetUserWithRolesAsync(string userId);
        Task<Result<BaseResponse<RoleWithUsersDto>>> GetRoleWithUsersAsync(string roleId, PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<PaginatedList<UserWithRolesDto>>>> GetUsersWithRolesAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default);
    }
}

