using Application.Common.Interfaces.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.DTOs;

namespace API.Controllers
{
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("UploadFile")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFile(FileInfoDTO file)
        {
            // Remove white spaces from the path
            file.Path = file.Path.Replace(" ", "_");  // or use Replace(" ", "") if you prefer to remove spaces completely

            string? url = null;
            if (file.FileData != null)
            {
                url = await _fileService.ConvertStreamToFileAsync(file.FileData, file.Path);
            }
            else if (file.Base64 != null)
            {
                url = await _fileService.ConvertBase64ToFileAsync(file.Base64, file.Path);
            }
            else if (file.Base64EncodedString != null)
            {
                url = await _fileService.ConvertBase64ToFileAsync(file.Base64EncodedString, file.Path);
            }

            return Result(new BaseResponse<string>(url));
        }


    }
}
