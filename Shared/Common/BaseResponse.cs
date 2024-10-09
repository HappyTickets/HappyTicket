using System.Collections;
using System.Net;

namespace Shared.Common;

public class BaseResponse
{
    public virtual dynamic? Data { get; set; }
    public string? Title { get; set; }
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public IEnumerable<ResponseError>? ErrorList { get; set; }
    public bool IsSuccess => (int)Status < 400;
}
public class BaseResponse<T>
{
    public BaseResponse() { }
    public BaseResponse(T data)
    {
        Data = data;
    }

    public virtual T? Data { get; set; }
    public string? Title { get; set; }
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public IEnumerable<ResponseError>? ErrorList { get; set; }
    public bool IsSuccess => (int)Status < 400;
}
public class BaseListResponse<T> where T : IEnumerable
{
    public BaseListResponse() { }
    public BaseListResponse(T data)
    {
        Data = data;
    }

    public virtual T? Data { get; set; }
    public string? Title { get; set; }
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public IEnumerable<ResponseError>? ErrorList { get; set; }
    public bool IsSuccess => (int)Status < 400;
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
