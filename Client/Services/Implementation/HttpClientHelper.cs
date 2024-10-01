using Blazored.LocalStorage;
using Client.Services.Interfaces;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared.Common.Enums;
using Shared.Common.General;
using Shared.Common.Interfaces;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.TokenDTOs;
using Shared.Exceptions;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Client.Services.Implementation
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IApiResponse _apiResponse;
        private readonly IAuthStateProvider _authStateProvider;
        public HttpClient _ScopedHttpClient { get; set; }

        

        public HttpClientHelper(ILocalStorageService localStorage, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, IApiResponse apiResponse)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _httpClient.BaseAddress = new Uri(UrlHelper.GetAPIBase());
            _apiResponse = apiResponse;
            _ScopedHttpClient = httpClient;
        }

        //public async Task<BaseResponse<Tout>> GetBaseAsync<Tout>(string url, Dictionary<string, string> queryParams, bool useAuth = true) where Tout : class
        //{
        //    try
        //    {
        //        if (useAuth) _ = await GetUserCurrentIdentityAsync();

        //        StringBuilder queryBuilder = new($"{URLHelper.GetAPIBase()}{url}");
        //        if (queryParams.Any())
        //        {
        //            queryBuilder.Append('?');
        //            foreach (var param in queryParams)
        //            {
        //                queryBuilder.Append($"{param.Key}={param.Value}");
        //                if (param.Key != queryParams.Last().Key) queryBuilder.Append('&');
        //            }
        //        }

        //        var response = await _httpClient.GetAsync(queryBuilder.ToString());
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var deserializedResponse = JsonConvert.DeserializeObject<BaseResponse<Tout>>(responseContent);

        //        if (deserializedResponse != null)
        //            return deserializedResponse;
        //        else
        //            return new BaseResponse<Tout>
        //            {
        //                Status = HttpStatusCode.InternalServerError,
        //                Title = $"Couldn't deserialize the response to {typeof(Tout).Name}"
        //            };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<Tout>
        //        {
        //            Status = HttpStatusCode.InternalServerError,
        //            Title = ex.Message,
        //            ErrorList = new List<ResponseError>
        //    {
        //        new ResponseError
        //        {
        //            Title = "Exception",
        //            Message = ex.Message
        //        }
        //    }
        //        };
        //    }
        //}

        public async Task<ApiResponse> GetAsync<T>(string Url, bool useAuth = true)
        {
            if (useAuth) _ = await GetUserCurrentIdentityAsync();

            var Response = await _httpClient.GetAsync(@$"{UrlHelper.GetAPIBase()}" + Url);

            Response.EnsureSuccessStatusCode();

            if (Response.IsSuccessStatusCode)
            {
                var responseContent = await Response.Content.ReadFromJsonAsync<ApiResponse>();
                if (responseContent.Data != null)
                {
                    var result = JsonConvert.DeserializeObject<T>(responseContent.Data.ToString());
                    responseContent.Data = result;
                }
                return responseContent;
            }
            else
            {
                return _apiResponse.GetApiResponse(CustomCodeStatus.UserNotFound, System.Net.HttpStatusCode.BadGateway, "Error...");
            }
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7211/DeleteMatch/{Id}");
            var content = await response.Content.ReadAsStringAsync();

            bool result;
            if (bool.TryParse(content, out result))
            {
                return result;
            }
            return result;
        }

        public async Task<ApiResponse> PostAsync(object T, string Url, bool useAuth = true)
        {
            if (useAuth) _ = await GetUserCurrentIdentityAsync();

            var Add = System.Text.Json.JsonSerializer.Serialize(T);
            var response = await _httpClient.PostAsync(@$"{UrlHelper.GetAPIBase()}" + Url,
                new StringContent(Add, Encoding.UTF8, "application/json"));

            var Res = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (Res != null)
                return Res;
            else
            {
                return _apiResponse.GetApiResponse(CustomCodeStatus.SomethingWentWrong, System.Net.HttpStatusCode.InternalServerError, "Error...");
            }
        }

        public async Task<ApiResponse> PutAsync(object T, string Url, bool useAuth = true)
        {
            if (useAuth) _ = await GetUserCurrentIdentityAsync();

            var content = System.Text.Json.JsonSerializer.Serialize(T);
            var x = UrlHelper.GetAPIBase() + Url;
            var response = await _httpClient.PutAsync($"{UrlHelper.GetAPIBase()}{Url}",
                new StringContent(content, Encoding.UTF8, "application/json"));

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (deserializedResponse != null)
                return deserializedResponse;
            else
            {
                return _apiResponse.GetApiResponse(CustomCodeStatus.SomethingWentWrong, System.Net.HttpStatusCode.InternalServerError, "Error...");
            }
        }

        public async Task<Result<Tout>> GetBaseAsync<Tout>(string url, IDictionary<string, string>? queryParams = null, bool useAuth = true)
        {
            try
            {
                if (useAuth) _ = await GetUserCurrentIdentityAsync();

                StringBuilder queryBuilder = new(url);
                if (queryParams != null && queryParams.Any())
                {
                    queryBuilder = queryBuilder.Append('?');
                    foreach (var param in queryParams)
                    {
                        queryBuilder = queryBuilder.Append($"{param.Key}={param.Value}");
                        if (!string.Equals(param.Key, queryParams.Last().Key, StringComparison.OrdinalIgnoreCase)) queryBuilder = queryBuilder.Append('&');
                    }
                }

                var response = await _httpClient.GetAsync(queryBuilder.ToString());
                var responseContent = await response.Content.ReadAsStringAsync();

                var deserializedResponse = JsonConvert.DeserializeObject<Tout>(responseContent);
                if (deserializedResponse != null)
                    return deserializedResponse;

                return new(new Exception($"Couldn't deserialize the response to {typeof(Tout).Name}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new(ex);
            }
        }


        public async Task<Result<Tout>> PostBaseAsync<Tin, Tout>(string url, Tin entity, bool useAuth = true)
        {
            try
            {
                if (useAuth) _ = await GetUserCurrentIdentityAsync();

                var content = JsonConvert.SerializeObject(entity);
                var response = await _httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<Tout>(responseContent);
                if (deserializedResponse != null)
                    return deserializedResponse;
                return new(new Exception($"Couldn't deserialize the response to {typeof(Tout).Name}"));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<Tout>> PutBaseAsync<Tin, Tout>(string url, Tin entity, bool useAuth = true)
        {
            try
            {
                if (useAuth) _ = await GetUserCurrentIdentityAsync();

                var content = JsonConvert.SerializeObject(entity);
                var response = await _httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                var responseContent = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<Tout>(responseContent);
                if (deserializedResponse != null)
                    return deserializedResponse;
                return new(new Exception($"Couldn't deserialize the response to {typeof(Tout).Name}"));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<ClaimsIdentity> GetUserCurrentIdentityAsync()
        {
            // Retrieve the JWT token from local storage
            string? token = await _localStorage.GetItemAsStringAsync("jwtToken");

            // Remove enclosing quotes from token if present
            if (token != null && token.StartsWith("\"") && token.EndsWith("\""))
            {
                token = token.Substring(1, token.Length - 2);
            }

            var claims = ParseClaimsFromJwt(token);

            if (claims.Any())
            {
                // Extract the expiration date from the claims
                string expirationDateStr = claims.FirstOrDefault(x =>
                    x.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;

                bool isValid = long.TryParse(expirationDateStr, out long numDate);
                DateTimeOffset expirationDate = DateTimeOffset.FromUnixTimeSeconds(numDate).UtcDateTime;

                // Check if the token is expired
                if (!isValid || DateTimeOffset.UtcNow > expirationDate)
                {
                    var refreshResult = await RefreshAuthAsync();
                    return await refreshResult.Match(
                        async succ =>
                        {
                            // Remove enclosing quotes from refreshed JWT if present
                            string refreshedToken = succ.JWT;
                            if (refreshedToken.StartsWith("\"") && refreshedToken.EndsWith("\""))
                            {
                                refreshedToken = refreshedToken.Substring(1, refreshedToken.Length - 2);
                            }

                            // Create a new ClaimsIdentity with the refreshed token
                            var identity = new ClaimsIdentity(ParseClaimsFromJwt(refreshedToken), "jwt");
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken);
                            await _authenticationStateProvider.GetAuthenticationStateAsync();
                            return identity;
                        },
                        async fail =>
                        {
                            // Handle failure to refresh the token
                            _httpClient.DefaultRequestHeaders.Authorization = null;
                            await _authenticationStateProvider.GetAuthenticationStateAsync();
                            return new ClaimsIdentity(); // Return an empty identity
                        });
                }

                // Return the existing ClaimsIdentity if the token is still valid
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return new ClaimsIdentity(claims, "jwt");
            }

            // If no claims were found, set authorization header to null and return an empty identity
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _authenticationStateProvider.GetAuthenticationStateAsync();
            return new ClaimsIdentity(); // Return an empty identity
        }


        private async Task<Result<TokenDTO>> RefreshAuthAsync()
        {
            var jwtToken = await _localStorage.GetItemAsync<string>("jwtToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            if (jwtToken == null || refreshToken == null) return new(new BadRequestException());

            var refreshAuthTokenResponse = await RefreshAuthToken(new() { AuthInfo = new() { JWT = jwtToken, RefreshToken = refreshToken } });
            return await refreshAuthTokenResponse.Match(
                async succ =>
                {
                    if (succ.IsSuccess && succ.Data != null && !string.IsNullOrWhiteSpace(succ.Data.JWT) && !string.IsNullOrWhiteSpace(succ.Data.RefreshToken))
                        _authStateProvider.SetAuthenticationState(true);

                    {
                        await _localStorage.SetItemAsync("jwtToken", succ.Data.JWT);
                        await _localStorage.SetItemAsync("refreshToken", succ.Data.RefreshToken);
                        return succ.Data;
                    }
                    await _localStorage.RemoveItemAsync("jwtToken");
                    await _localStorage.RemoveItemAsync("refreshToken");
                    await LogoutAsync(new() { UserInfo = new() { JWT = jwtToken, RefreshToken = refreshToken } });
                    return new Result<TokenDTO>(new BaseException() { Code = succ.Status, Errors = succ.ErrorList?.Select(x => new ErrorInfo() { Title = x.Title, Message = x.Message, Details = x.Details }) ?? [] });
                },
                async fail =>
                {
                    await _localStorage.RemoveItemAsync("jwtToken");
                    await _localStorage.RemoveItemAsync("refreshToken");
                    await LogoutAsync(new() { UserInfo = new() { JWT = jwtToken, RefreshToken = refreshToken } });
                    return new(fail);
                });
        }

        private async Task<Result<RefreshAuthTokenResponse>> RefreshAuthToken(RefreshAuthTokenRequest refreshAuthTokenRequest)
        {
            return await PostBaseAsync<RefreshAuthTokenRequest, RefreshAuthTokenResponse>("api/Identity/RefreshAuthToken", refreshAuthTokenRequest, false);
        }

        private async Task<Result<LogoutResponse>> LogoutAsync(LogoutRequest logoutRequest)
        {
            return await PostBaseAsync<LogoutRequest, LogoutResponse>("api/Identity/Logout", logoutRequest, false);
        }

        private static List<Claim> ParseClaimsFromJwt(string? jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt) || !jwt.Any(x => x == '.')) return [];
            var payload = jwt.Split('.')[1];
            var json = Encoding.UTF8.GetString(ParseBase64WithoutPadding(payload));
            var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)).ToList() ?? [];
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            try
            {
                Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
                if (Convert.TryFromBase64String(base64, buffer, out int bytesParsed))
                {
                    return buffer.ToArray();
                }
                base64 = base64.Replace('_', '/').Replace('-', '+');
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
                if (Convert.TryFromBase64String(base64, buffer, out bytesParsed))
                {
                    return buffer.ToArray();
                }
                return [];
            }
            catch (Exception ex)
            {
                return [];
            }
        }
    }
}
