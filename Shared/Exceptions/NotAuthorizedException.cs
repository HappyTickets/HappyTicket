using Shared.ResourceFiles;
using System.Net;

namespace Shared.Exceptions
{
    public class NotAuthorizedException : BaseException
    {
        public NotAuthorizedException() : base(Resource.NotAuthorized) { }
        public NotAuthorizedException(string message) : base(message) { }
        public NotAuthorizedException(IEnumerable<ErrorInfo> errors) : base(errors, Resource.NotAuthorized) { }
        public NotAuthorizedException(IEnumerable<ErrorInfo> errors, string message) : base(errors, message) { }

        public override HttpStatusCode Code { get; set; } = HttpStatusCode.Unauthorized;
    }

}
