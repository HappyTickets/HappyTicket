using Shared.Common.Enums;
using Shared.Common.General;
using System.Net;

namespace Shared.Common.Interfaces
{
    public interface IApiResponse : IDisposable
    {
        ApiResponse GetApiResponse();
        ApiResponse GetApiResponse(CustomCodeStatus errorNumber, HttpStatusCode httpStatusCode, string message = null, object data = null);
    }
}
