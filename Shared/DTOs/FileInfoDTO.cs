using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class FileInfoDTO
    {
        [Required]
        public string Path { get; set; } = string.Empty;
        public byte[]? Base64 { get; set; }
        public string? Base64EncodedString { get; set; }
        public IBrowserFile? FileData { get; set; }
    }
}
