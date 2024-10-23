using Shared.Common;
using Shared.DTOs;

namespace Client.Services._File
{
    public interface IFileService
    {
        Task<BaseResponse<string>> UploadFileAsync(FileInfoDTO file, CancellationToken cancellationToken = default);
    }
}
