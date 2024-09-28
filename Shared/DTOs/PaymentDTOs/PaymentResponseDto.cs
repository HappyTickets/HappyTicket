using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Shared.Common;

namespace Shared.DTOs.PaymentDTOs
{
    public class PaymentResponseDto
    {
        public string? PaymentUrl { get; set; }

        public string? PaymentRef { get; set; }

        public string? AuthorizedToken { get; set; }

        public string? DeclinedToken { get; set; }

        public string? CancelledToken { get; set; }

        public bool HasErrors { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}
