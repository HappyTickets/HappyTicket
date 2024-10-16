//using API.Attributes;
//using LanguageExt.Common;
//using Microsoft.AspNetCore.Mvc;
//using Shared.Common;
//using Shared.Exceptions;
//using System.Collections;
//using System.Net;

//namespace API.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[ValidationActionFilter]
//public class BaseController : ControllerBase
//{
//    protected HttpContext? _httpContext;
//    protected IHttpContextAccessor _httpContextAccessor;

//    public BaseController(IHttpContextAccessor httpContextAccessor)
//    {
//        _httpContextAccessor = httpContextAccessor;
//        _httpContext = httpContextAccessor.HttpContext;
//    }

//    [NonAction]
//    public ObjectResult ReturnResult<T>(Result<T> result)
//    {
//        return result.Match(
//                succ => IsEnumerable(typeof(T)) ? ReturnRequest(new BaseResponse() { Status = HttpStatusCode.OK, Data = succ }) : ReturnRequest(new BaseResponse<T>() { Status = HttpStatusCode.OK, Data = succ }),
//                fail => (fail is BaseException baseException) ? ReturnException(baseException) : ReturnException(fail));
//    }
//    [NonAction]
//    public ObjectResult ReturnListResult<T>(Result<T> result) where T : IEnumerable
//    {
//        return result.Match(
//                succ => ReturnListRequest(new BaseListResponse<T>() { Status = HttpStatusCode.OK, Data = succ }),
//                fail => (fail is BaseException baseException) ? ReturnException(baseException) : ReturnException(fail));
//    }

//    [NonAction]
//    public ObjectResult ReturnBaseResult<R>(Result<R> result) where R : BaseResponse
//    {
//        return result.Match(
//                ReturnRequest,
//                fail => (fail is BaseException baseException) ? ReturnException(baseException) : ReturnException(fail));
//    }
//    [NonAction]
//    public ObjectResult ReturnResult<R, T>(Result<R> result)
//        where R : BaseResponse<T>
//    {
//        return result.Match(
//                ReturnRequest,
//                fail => (fail is BaseException baseException) ? ReturnException(baseException) : ReturnException(fail));
//    }
//    [NonAction]
//    public ObjectResult ReturnListResult<R, T>(Result<R> result)
//        where R : BaseListResponse<T>
//        where T : IEnumerable
//    {
//        return result.Match(
//                ReturnListRequest,
//                fail => (fail is BaseException baseException) ? ReturnException(baseException) : ReturnException(fail));
//    }

//    [NonAction]
//    public ObjectResult ReturnRequest(BaseResponse baseResponse)
//    {
//        if (baseResponse.Status == HttpStatusCode.NotFound || (int)baseResponse.Status < 400)
//        {
//            return Ok(baseResponse);
//        }
//        else
//        {
//            return BadRequest(baseResponse);
//        }
//    }
//    [NonAction]
//    public ObjectResult ReturnRequest<T>(BaseResponse<T> baseResponse)
//    {
//        if (baseResponse.Status == HttpStatusCode.NotFound || (int)baseResponse.Status < 400)
//        {
//            return Ok(baseResponse);
//        }
//        else
//        {
//            return BadRequest(baseResponse);
//        }
//    }
//    [NonAction]
//    public ObjectResult ReturnListRequest<T>(BaseListResponse<T> baseListResponse) where T : IEnumerable
//    {
//        if (baseListResponse.Status == HttpStatusCode.NotFound || (int)baseListResponse.Status < 400)
//        {
//            return Ok(baseListResponse);
//        }
//        else
//        {
//            return BadRequest(baseListResponse);
//        }
//    }

//    [NonAction]
//    public ObjectResult ReturnException(Exception ex)
//    {
//        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
//        {
//            Status = HttpStatusCode.InternalServerError,
//            Title = ex.InnerException != null ? ex.InnerException.Message : ex.Message
//        });
//    }
//    [NonAction]
//    public ObjectResult ReturnException(BaseException ex)
//    {
//        if (ex is NotFoundException notFound) return ReturnRequest(new BaseResponse() { Status = HttpStatusCode.NoContent, Title = notFound.Message, ErrorList = notFound.Errors.Select(x => new ResponseError() { Title = x.Title, Message = x.Message, Details = x.Details }) });
//        return StatusCode((int)ex.Code, new BaseResponse
//        {
//            Status = ex.Code,
//            Title = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
//            ErrorList = ex.Errors.Select(x => new ResponseError() { Title = x.Title, Message = x.Message, Details = x.Details })
//        });
//    }

//    [NonAction]
//    private static bool IsEnumerable(Type type) => type.IsArray || type.GetInterfaces().Any(x => x.Name == nameof(IEnumerable) || x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable));
//}
