using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface IFileService
    {
        Task<Result<BaseResponse<string>>> UploadFileAsync(FileInfoDTO file, CancellationToken cancellationToken = default);
    }
}
