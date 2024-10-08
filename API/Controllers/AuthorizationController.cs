using Application.Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.ApiRoutes;
using Shared.DTOs.Authorization.Request;
using System.Net;

namespace API.Controllers
{
    public class AuthorizationController(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor) : BaseController(httpContextAccessor)
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;



        [HttpPost(ApiRoutes.Authorization.CreateRole)]
        public async Task<IActionResult> Create([FromBody] AddRoleDto addRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return ReturnResult(await _authorizationService.AddRoleAsync(addRoleDto));

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(ApiRoutes.Authorization.EditRole)]
        public async Task<IActionResult> Edit([FromBody] EditRoleDto editRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return ReturnResult(await _authorizationService.EditRoleAsync(editRoleDto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Authorization.DeleteRole)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                return ReturnResult(await _authorizationService.DeleteRoleAsync(id));

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Authorization.RolesList)]
        public async Task<IActionResult> GetRolesList()
        {
            try
            {
                return ReturnResult(await _authorizationService.GetRolesList());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] string id)
        {
            try
            {
                return ReturnResult(await _authorizationService.GetRoleById(id));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(ApiRoutes.Authorization.AssignUsersToRole)]
        public async Task<IActionResult> AssignUsersToRole([FromBody] AssignUsersToRoleDto assignUsersToRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return ReturnResult(await _authorizationService.AssignUsersToRoleAsync(assignUsersToRoleDto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(ApiRoutes.Authorization.AssignUserToRoles)]
        public async Task<IActionResult> AssignUserToRoles([FromBody] AssignUserToRolesDto assignUserToRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return ReturnResult(await _authorizationService.AssignUserToRolesAsync(assignUserToRoleDto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Remove a user from a role
        [HttpPost(ApiRoutes.Authorization.RemoveUsersFromRole)]
        public async Task<IActionResult> RemoveUsersFromRole([FromBody] RemoveUsersFromRoleDto removeUserFromRoleDto)
        {
            try
            {
                return ReturnResult(await _authorizationService.RemoveUsersFromRoleAsync(removeUserFromRoleDto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Get a user with their roles
        [HttpGet(ApiRoutes.Authorization.GetUserWithRoles)]
        public async Task<IActionResult> GetUserWithRoles([FromRoute] string userId)
        {
            try
            {
                return ReturnResult(await _authorizationService.GetUserWithRolesAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Authorization.GetUsersWithRoles)]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            try
            {
                return ReturnListResult(await _authorizationService.GetUsersWithRolesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Get a role with its users
        [HttpGet(ApiRoutes.Authorization.GetRoleWithUsers)]
        public async Task<IActionResult> GetRoleWithUsers([FromRoute] string roleId)
        {
            try
            {
                return ReturnResult(await _authorizationService.GetRoleWithUsersAsync(roleId));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
