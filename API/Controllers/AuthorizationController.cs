using Application.Permissions.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.ApiRoutes;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;

namespace API.Controllers
{
    public class AuthorizationController(IAuthorizationService authorizationService) : BaseController
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;



        [HttpPost(ApiRoutes.Authorization.CreateRole)]
        public async Task<IActionResult> Create([FromBody] AddRoleDto addRoleDto)
        {


            return Result(await _authorizationService.AddRoleAsync(addRoleDto));


        }

        [HttpPost(ApiRoutes.Authorization.EditRole)]
        public async Task<IActionResult> Edit([FromBody] EditRoleDto editRoleDto)
        {

            return Result(await _authorizationService.EditRoleAsync(editRoleDto));

        }


        [HttpGet(ApiRoutes.Authorization.DeleteRole)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {

            return Result(await _authorizationService.DeleteRoleAsync(id));

        }



        [HttpGet(ApiRoutes.Authorization.RolesList)]
        public async Task<IActionResult> GetRolesList()
        {

            return Result(await _authorizationService.GetRolesList());

        }

        [HttpGet(ApiRoutes.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] long id)
        {

            return Result(await _authorizationService.GetRoleById(id));

        }

        [HttpPost(ApiRoutes.Authorization.AssignUsersToRole)]
        public async Task<IActionResult> AssignUsersToRole([FromBody] AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default)
        {


            return Result(await _authorizationService.AssignUsersToRoleAsync(assignUsersToRoleDto, cancellationToken));

        }


        [HttpPost(ApiRoutes.Authorization.AssignUserToRoles)]
        public async Task<IActionResult> AssignUserToRoles([FromBody] AssignUserToRolesDto assignUserToRoleDto)
        {

            return Result(await _authorizationService.AssignUserToRolesAsync(assignUserToRoleDto));

        }


        [HttpPost(ApiRoutes.Authorization.RemoveUsersFromRole)]
        public async Task<IActionResult> RemoveUsersFromRole([FromBody] RemoveUsersFromRoleDto removeUserFromRoleDto, CancellationToken cancellationToken = default)
        {
            return Result(await _authorizationService.RemoveUsersFromRoleAsync(removeUserFromRoleDto, cancellationToken));

        }

        // Get a user with their roles
        [HttpGet(ApiRoutes.Authorization.GetUserWithRoles)]
        public async Task<IActionResult> GetUserWithRoles([FromRoute] long userId)
        {

            return Result(await _authorizationService.GetUserWithRolesAsync(userId));

        }


        [HttpGet(ApiRoutes.Authorization.GetUsersWithRoles)]
        public async Task<IActionResult> GetUsersWithRoles([FromQuery] PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default)
        {
            return Result(await _authorizationService.GetUsersWithRolesAsync(paginationSearchModel, cancellationToken));
        }


        [HttpGet(ApiRoutes.Authorization.GetRoleWithUsers)]
        public async Task<IActionResult> GetRoleWithUsers([FromRoute] long roleId, [FromQuery] PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.GetRoleWithUsersAsync(roleId, paginationSearchModel, cancellationToken);
            return Result(result);

        }

    }
}

