using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.OrderDtos.Response
{
    public class OrderOwner
    {
        public long Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class OrderDto
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; }

        public int? PaymentStatus { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? PaymentOrderRef { get; set; }

        public string? PaymentUrl { get; set; }

        public OrderOwner Owner { get; set; }

        public IEnumerable<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

}
