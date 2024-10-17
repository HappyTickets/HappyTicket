using API.Attributes;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Exceptions;
using System.Collections;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ValidationActionFilter]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Result(BaseResponse<object?> baseResponse)
        => StatusCode((int)baseResponse.Status, baseResponse);
}
