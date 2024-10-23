using Azure;
using Shared.Common;

namespace Client.Services._HttpClientFacade
{
    public interface IHttpClientFacade
    {
        Task<TResponse> PostAsync<TResponse>(string url, object data, bool useAuth = true);
        Task<TResponse> PutAsync<TResponse>(string url, object data, bool useAuth = true);
        Task<TResponse> DeleteAsync<TResponse>(string url, bool useAuth = true);
        Task<TResponse> GetAsync<TResponse>(string url, bool useAuth = true);
    }
}
