using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public FileService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        public async Task<Result<BaseResponse<string>>> UploadFileAsync(FileInfoDTO file, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PostBaseAsync<FileInfoDTO, BaseResponse<string>>($"api/File/UploadFile", file);
        }
    }
}
