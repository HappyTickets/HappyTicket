using System.Net;

namespace Shared.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException() : base("Item Not Found") { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(IEnumerable<ErrorInfo> errors) : base(errors, "Item Not Found") { }
    public NotFoundException(IEnumerable<ErrorInfo> errors, string message) : base(errors, message) { }

    public override HttpStatusCode Code { get; set; } = HttpStatusCode.NoContent;
}
