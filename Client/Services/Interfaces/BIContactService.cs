using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.ContactDto;

namespace Client.Services.Interfaces
{
    public interface BIContactService
    {
        Task<Result<BaseResponse<bool>>> SendMessageAsync(Contact contactModel, CancellationToken cancellationToken = default);
    }
}
