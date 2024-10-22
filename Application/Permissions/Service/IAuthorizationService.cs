using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Application.Permissions.Service
{
    public interface IAuthorizationService
    {
        Task<BaseResponse<Empty>> AddRoleAsync(AddRoleDto addRoleDto);
        Task<bool> IsRoleExistById(long roleId);
        Task<bool> IsRoleExist(string roleName);
        Task<BaseResponse<Empty>> EditRoleAsync(EditRoleDto request);
        Task<BaseResponse<Empty>> DeleteRoleAsync(long roleId);
        Task<BaseResponse<List<RoleDto>>> GetRolesList(CancellationToken cancellationToken = default);
        Task<BaseResponse<RoleDto>> GetRoleById(long id);

        Task<BaseResponse<Empty>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto);
        Task<BaseResponse<Empty>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default);
        Task<BaseResponse<Empty>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUsersFromRoleDto, CancellationToken cancellationToken = default);

        Task<BaseResponse<UserWithRolesDto>> GetUserWithRolesAsync(long userId);
        Task<BaseResponse<RoleWithUsersDto>> GetRoleWithUsersAsync(long roleId, PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default);
        Task<BaseResponse<PaginatedList<UserWithRolesDto>>> GetUsersWithRolesAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken);
    }

}