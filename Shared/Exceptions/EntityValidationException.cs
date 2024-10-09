using FluentValidation.Results;
using System.Net;

namespace Shared.Exceptions;

public class EntityValidationException : BaseException
{
    public EntityValidationException() : base() { }
    public EntityValidationException(IEnumerable<ValidationFailure> failures) : base()
    {
        Errors = failures.GroupBy(x => x.PropertyName).Select(e => new ErrorInfo() { Title = e.Key, Details = e.Select(em => em.ErrorMessage) });
    }
    public EntityValidationException(IEnumerable<ErrorInfo> errors) : base(errors)
    {

    }

    public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;
}
