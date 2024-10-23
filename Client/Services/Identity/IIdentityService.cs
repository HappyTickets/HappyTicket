using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.TokenDTOs;

namespace Client.Services.Identity
{
    public interface IIdentityService
    {
        Task<BaseResponse<TokenDTO>> LoginAsync(LoginRequest request);
        Task<BaseResponse<TokenDTO>> ReloginAsync();
        Task<BaseResponse<Empty>> LogoutAsync();
    }
}
