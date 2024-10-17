using Microsoft.AspNetCore.Components.Forms;

namespace Application.Interfaces.Infrastructure.Services
{
    public interface IFileService
    {
        Task<string> ConvertBase64ToFileAsync(byte[] base64encoded, string path);
        Task<string> ConvertBase64ToFileAsync(string base64encodedstring, string path);
        Task<string> ConvertStreamTobase64Async(IBrowserFile file);
        Task<string> ConvertStreamTobase64Async(Stream stream, string type = "image/jpeg");
        Task<string> ConvertStreamToFileAsync(IBrowserFile file, string path);
        Task<string> ConvertStreamToFileAsync(Stream stream, string path);
    }
}