//using Domain.Exceptions;
//using System.Net;

//namespace DTO.Shared;

//public class ExceptionResponse : BaseException
//{
//    public List<ResponseError>? ErrorList { get; set; }
//    public bool IsSuccess => ((int)ResponseCode < 400) || ErrorList == null || ErrorList.Count == 0;
//}
//public class BaseResponse<T>
//{
//    public BaseResponse() { }
//    public BaseResponse(T data)
//    {
//        Data = data;
//    }

//    public virtual T? Data { get; set; }
//    public string ResponseMessage { get; set; } = string.Empty;
//    public HttpStatusCode ResponseCode { get; set; }
//    public List<ResponseError>? ErrorList { get; set; }
//    public bool IsSuccess => ((int)ResponseCode < 400) || ErrorList == null || ErrorList.Count == 0;
//}
//public class ResponseError
//{
//    public string Title { get; set; } = string.Empty;
//    public string Message { get; set; } = string.Empty;
//}
