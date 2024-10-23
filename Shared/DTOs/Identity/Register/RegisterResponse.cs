using Shared.Common;
using Shared.DTOs.Identity.TokenDTOs;

namespace Shared.DTOs.Identity.Register;

public class RegisterResponse : BaseResponse<TokenDTO>
{
    public override TokenDTO? Data { get; set; }
}
