//using Client.Services.Interfaces;
//using LanguageExt;
//using LanguageExt.Common;
//using Shared.Common;
//using Shared.Common.ApiRoutes;
//using Shared.Common.General;
//using Shared.DTOs.Authorization.Request;
//using Shared.DTOs.Authorization.Response;

//namespace Client.Services.Implementation
//{
//    public class BAuthorizationService : BIAuthorizationService
//    {
//        private readonly IHttpClientHelper _httpClient;
//        private const string BASE = ApiRoutes.Authorization.Base;

//        public BAuthorizationService(IHttpClientHelper httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<Result<BaseResponse<Unit>>> AddRoleAsync(AddRoleDto addRoleDto)
//        {
//            return await _httpClient.PostBaseAsync<AddRoleDto, BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.CreateRole, addRoleDto);
//        }

//        public async Task<Result<BaseResponse<Unit>>> EditRoleAsync(EditRoleDto request)
//        {
//            return await _httpClient.PostBaseAsync<EditRoleDto, BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.EditRole, request);
//        }

//        public async Task<Result<BaseResponse<Unit>>> DeleteRoleAsync(string roleId)
//        {
//            return await _httpClient.GetBaseAsync<BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.DeleteRole.Replace("{id}", roleId));
//        }

//        public async Task<Result<BaseResponse<List<RoleDto>>>> GetRolesList(CancellationToken cancellationToken)
//        {
//            return await _httpClient.GetBaseAsync<BaseResponse<List<RoleDto>>>(BASE + ApiRoutes.Authorization.RolesList);
//        }

//        public async Task<Result<BaseResponse<RoleDto>>> GetRoleById(string id)
//        {
//            return await _httpClient.GetBaseAsync<BaseResponse<RoleDto>>(BASE + ApiRoutes.Authorization.GetRoleById.Replace("{id}", id));
//        }

//        public async Task<Result<BaseResponse<Unit>>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRoleDto)
//        {
//            return await _httpClient.PostBaseAsync<AssignUserToRolesDto, BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.AssignUserToRoles, assignUserToRoleDto);
//        }
//        public async Task<Result<BaseResponse<Unit>>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken)
//        {
//            return await _httpClient.PostBaseAsync<AssignUsersToRoleDto, BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.AssignUsersToRole, assignUsersToRoleDto);
//        }
//        public async Task<Result<BaseResponse<Unit>>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUserFromRoleDto, CancellationToken cancellationToken)
//        {
//            return await _httpClient.PostBaseAsync<RemoveUsersFromRoleDto, BaseResponse<Unit>>(BASE + ApiRoutes.Authorization.RemoveUsersFromRole, removeUserFromRoleDto);
//        }

//        public async Task<Result<BaseResponse<UserWithRolesDto>>> GetUserWithRolesAsync(string userId)
//        {
//            return await _httpClient.GetBaseAsync<BaseResponse<UserWithRolesDto>>(BASE + ApiRoutes.Authorization.GetUserWithRoles.Replace("{userId}", userId));
//        }

//        public async Task<Result<BaseResponse<RoleWithUsersDto>>> GetRoleWithUsersAsync(string roleId, PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default)
//        {
//            var queryParameters = paginationSearchModel.AsDictionary();

//            return await _httpClient.GetBaseAsync<BaseResponse<RoleWithUsersDto>>(BASE + ApiRoutes.Authorization.GetRoleWithUsers.Replace("{roleId}", roleId), queryParameters);
//        }

//        public async Task<Result<BaseResponse<PaginatedList<UserWithRolesDto>>>> GetUsersWithRolesAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken)
//        {
//            var queryParameters = paginationSearchModel.AsDictionary();

//            return await _httpClient.GetBaseAsync<BaseResponse<PaginatedList<UserWithRolesDto>>>

//                (BASE + ApiRoutes.Authorization.GetUsersWithRoles, queryParameters);
//        }


//    }
//}
