using Shared.DTOs.CartDTOs;
using Shared.DTOs.Identity.UserDTOs;

namespace Shared.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUserDTO User { get; set; } = new ApplicationUserDTO();
        public decimal TotalAmount { get; set; }
        public int? PaymentStatus { get; set; }
        public string? PaymentOrderRef { get; set; }
        public string? PaymentStatusString { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? SearchKey { get; set; }
        public string? SearchIn { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; } = [];
    }

}
