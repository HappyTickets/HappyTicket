using Client.Services._HttpClientFacade;
using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs;

namespace Client.Services._File
{
    public class FileService : IFileService
    {
        private readonly IHttpClientFacade _httpClientFacade;

        public FileService(IHttpClientFacade httpClientFacade)
        {
            _httpClientFacade = httpClientFacade;
        }

        public async Task<BaseResponse<string>> UploadFileAsync(FileInfoDTO file, CancellationToken cancellationToken = default)
            => await _httpClientFacade.PostAsync<BaseResponse<string>>($"api/file/uploadFile", file);
    }
}
