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



        [HttpPost(ApiRoutes.Authorization.Create)]
        public async Task<IActionResult> Create([FromForm] AddRoleDto addRoleDto)
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

        [HttpPost(ApiRoutes.Authorization.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditRoleDto editRoleDto)
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

        [HttpDelete(ApiRoutes.Authorization.Delete)]
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

    }
}
