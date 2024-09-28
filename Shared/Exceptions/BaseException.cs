using System.Net;

namespace Shared.Exceptions;

public class BaseException : Exception
{
    public virtual HttpStatusCode Code { get; set; }
    public IEnumerable<ErrorInfo> Errors { get; set; } = [];

    public BaseException() { }
    public BaseException(string message) : base(message) { }
    public BaseException(IEnumerable<ErrorInfo> errors)
    {
        Errors = errors;
    }
    public BaseException(IEnumerable<ErrorInfo> errors, string message) : base(message)
    {
        Errors = errors;
    }
}
public class ErrorInfo
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string> Details { get; set; } = [];
}
