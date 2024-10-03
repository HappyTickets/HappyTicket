using Application.Implementations.IdentityServices;
using Application.Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.ApiRoutes;
using Shared.Common.General;
using System.Net;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IHttpContextAccessor httpContextAccessor, IUserService userService) : base(httpContextAccessor)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route(ApiRoutes.User.GetAll)]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationSearchModel queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.GetAllUsersAsync(queryParams, cancellationToken);
                return ReturnResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
