//using Application.Interfaces.IIdentityServices;
//using Microsoft.AspNetCore.Mvc;
//using Shared.Common.ApiRoutes;
//using Shared.Common.General;
//using Shared.DTOs.Authorization.Request;
//using System.Net;

//namespace API.Controllers
//{
//    public class AuthorizationController(IAuthorizationService authorizationService) : BaseController
//    {
//        private readonly IAuthorizationService _authorizationService = authorizationService;



//        [HttpPost(ApiRoutes.Authorization.CreateRole)]
//        public async Task<IActionResult> Create([FromBody] AddRoleDto addRoleDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                return ReturnResult(await _authorizationService.AddRoleAsync(addRoleDto));

//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPost(ApiRoutes.Authorization.EditRole)]
//        public async Task<IActionResult> Edit([FromBody] EditRoleDto editRoleDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                return ReturnResult(await _authorizationService.EditRoleAsync(editRoleDto));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//        [HttpGet(ApiRoutes.Authorization.DeleteRole)]
//        public async Task<IActionResult> Delete([FromRoute] string id)
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.DeleteRoleAsync(id));

//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }



//        [HttpGet(ApiRoutes.Authorization.RolesList)]
//        public async Task<IActionResult> GetRolesList()
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.GetRolesList());
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpGet(ApiRoutes.Authorization.GetRoleById)]
//        public async Task<IActionResult> GetRoleById([FromRoute] string id)
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.GetRoleById(id));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPost(ApiRoutes.Authorization.AssignUsersToRole)]
//        public async Task<IActionResult> AssignUsersToRole([FromBody] AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                return ReturnResult(await _authorizationService.AssignUsersToRoleAsync(assignUsersToRoleDto, cancellationToken));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//        [HttpPost(ApiRoutes.Authorization.AssignUserToRoles)]
//        public async Task<IActionResult> AssignUserToRoles([FromBody] AssignUserToRolesDto assignUserToRoleDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                return ReturnResult(await _authorizationService.AssignUserToRolesAsync(assignUserToRoleDto));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//        [HttpPost(ApiRoutes.Authorization.RemoveUsersFromRole)]
//        public async Task<IActionResult> RemoveUsersFromRole([FromBody] RemoveUsersFromRoleDto removeUserFromRoleDto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.RemoveUsersFromRoleAsync(removeUserFromRoleDto, cancellationToken));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        // Get a user with their roles
//        [HttpGet(ApiRoutes.Authorization.GetUserWithRoles)]
//        public async Task<IActionResult> GetUserWithRoles([FromRoute] string userId)
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.GetUserWithRolesAsync(userId));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//        [HttpGet(ApiRoutes.Authorization.GetUsersWithRoles)]
//        public async Task<IActionResult> GetUsersWithRoles([FromQuery] PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                return ReturnResult(await _authorizationService.GetUsersWithRolesAsync(paginationSearchModel, cancellationToken));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//        [HttpGet(ApiRoutes.Authorization.GetRoleWithUsers)]
//        public async Task<IActionResult> GetRoleWithUsers([FromRoute] string roleId, [FromQuery] PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var result = await _authorizationService.GetRoleWithUsersAsync(roleId, paginationSearchModel, cancellationToken);
//                return ReturnResult(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }


//    }
//}

