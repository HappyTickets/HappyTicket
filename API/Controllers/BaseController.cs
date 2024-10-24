using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[ValidationActionFilter]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Result<TData>(BaseResponse<TData> baseResponse)
        => baseResponse.Status switch
        {
            HttpStatusCode.NoContent => NoContent(),
            _ => StatusCode((int)baseResponse.Status, baseResponse)
        };
}
