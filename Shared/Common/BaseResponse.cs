using Shared.Exceptions;
using System.Net;

namespace Shared.Common;

public class BaseResponse<TData>
{
    public BaseResponse(HttpStatusCode status = HttpStatusCode.OK)
    {
        Status = status;
    }

    public BaseResponse(TData data, HttpStatusCode status = HttpStatusCode.OK)
    {
        Data = data;
        Status = status;
    }

    public virtual TData? Data { get; set; }
    public string? Title { get; set; }
    public HttpStatusCode Status { get; set; }
    public IEnumerable<ResponseError>? ErrorList { get; set; }
    public bool IsSuccess => (int)Status < 400;


    public static implicit operator BaseResponse<TData>(TData data)
       => new BaseResponse<TData>
       {
           Status = HttpStatusCode.OK,
           Data = data
       };

    public static implicit operator BaseResponse<TData>(BaseException ex)
        => new BaseResponse<TData>
        {
            Status = ex.Code,
            Title = ex.Message,
            ErrorList = ex.Errors.Select(x => new ResponseError() { Title = x.Title, Message = x.Message, Details = x.Details })
        };
}

public class ResponseError
{
    public ResponseError() { }
    public ResponseError(string title, string message)
    {
        Title = title;
        Message = message;
    }
    public ResponseError(string title, string message, IEnumerable<string> details)
    {
        Title = title;
        Message = message;
        Details = details;
    }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string> Details { get; set; } = new List<string>(); // DO NOT CHANGE `new List<string>()` TO `[]`. Newtonsoft.Json.JsonSerializationException
}
