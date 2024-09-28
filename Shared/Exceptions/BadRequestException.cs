using System.Net;

namespace Shared.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException() : base() { }
    public BadRequestException(IEnumerable<ErrorInfo> errors) : base(errors)
    {

    }
    public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;
}
