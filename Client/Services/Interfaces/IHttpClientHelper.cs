using LanguageExt.Common;
using Shared.Common.General;
using System.Security.Claims;

namespace Client.Services.Interfaces
{
    public interface IHttpClientHelper
    {
        public HttpClient _ScopedHttpClient { get; set; }

        Task<ApiResponse> GetAsync<T>(string Url, bool useAuth = true);
        Task<ApiResponse> PostAsync(object T, string Url, bool useAuth = true);
        Task<ApiResponse> PutAsync(object T, string Url, bool useAuth = true);

        //Task<BaseResponse<Tout>> GetBaseAsync<Tout>(string url, Dictionary<string, string> queryParams, bool useAuth = true) where Tout : class;
        Task<Result<Tout>> GetBaseAsync<Tout>(string url, IDictionary<string, string>? queryParams = null, bool useAuth = true);
        Task<Result<Tout>> PostBaseAsync<Tin, Tout>(string url, Tin entity, bool useAuth = true);
        Task<Result<Tout>> PutBaseAsync<Tin, Tout>(string url, Tin entity, bool useAuth = true);
        Task<ClaimsIdentity> GetUserCurrentIdentityAsync();
        Task<Result<Tout>> DeleteBaseAsync<Tout>(string url, bool useAuth = true);
    }
}
