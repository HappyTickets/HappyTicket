using System.Net;

namespace Shared.Exceptions;

public class ServerException : BaseException
{
    public ServerException() : base() { }
    public ServerException(IEnumerable<ErrorInfo> errors) : base(errors)
    {

    }

    public override HttpStatusCode Code { get; set; } = HttpStatusCode.InternalServerError;
}
