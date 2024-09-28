using Shared.Common;
using Shared.DTOs.Identity.TokenDTOs;

namespace Shared.DTOs.Identity.RefreshAuthToken;

public class RefreshAuthTokenResponse : BaseResponse<TokenDTO>
{
    public override TokenDTO? Data { get; set; }
}
