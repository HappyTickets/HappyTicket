using Shared.Common;
using Shared.DTOs.Identity.TokenDTOs;

namespace Shared.DTOs.Identity.Login;

public class LoginResponse : BaseResponse<TokenDTO>
{
    public override TokenDTO? Data { get; set; }
}
