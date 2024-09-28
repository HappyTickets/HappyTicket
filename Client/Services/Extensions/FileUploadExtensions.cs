using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace Client.Services.Extensions
{
    public static class FileUploadExtensions
    {
        public static async Task<string> ConvertStreamTobase64Async(this IBrowserFile file)
        {
            using var stream = file.OpenReadStream(10000000);
            return await stream.ConvertStreamTobase64Async(file.ContentType);
        }
        public static async Task<string> ConvertStreamTobase64Async(this Stream stream, string type = "image/jpeg")
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(bytes);
            return $"data:{type};base64,{base64String}";
        }
        public static async Task<string> ConvertBase64ToFileAsync(this string base64encodedstring, string path)
        {
            base64encodedstring = base64encodedstring.Split(',', 2)[1];
            return await ConvertBase64ToFileAsync(Convert.FromBase64String(base64encodedstring), path);
        }
        public static async Task<string> ConvertBase64ToFileAsync(this byte[] base64encoded, string path)
        {
            Stream memStream = new MemoryStream(base64encoded);
            return await ConvertStreamToFileAsync(memStream, path);
        }
        public static async Task<string> ConvertStreamToFileAsync(this IBrowserFile file, string path)
        {
            using var stream = file.OpenReadStream(10000000);
            return await stream.ConvertStreamToFileAsync(Path.Combine(path, file.Name));
        }
        public static async Task<string> ConvertStreamToFileAsync(this Stream stream, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;
                var pathWithoutFileName = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(pathWithoutFileName)) return string.Empty;

                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                StringBuilder fullPath = new(wwwrootPath);
                fullPath.Append(pathWithoutFileName);

                Directory.CreateDirectory(fullPath.ToString());

                StringBuilder fileName = new();
                fileName.AppendFormat("{0} - {1}{2}".Normalize(), Path.GetFileNameWithoutExtension(path), Guid.NewGuid().ToString(), Path.GetExtension(path));

                string filePath = Path.Combine(fullPath.ToString(), fileName.ToString());

                using var fileStream = File.Create(filePath);
                await stream.CopyToAsync(fileStream);

                return Path.GetRelativePath(wwwrootPath, filePath);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
