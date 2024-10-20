using Shared.DTOs.OrderDtos.Request;
using System.ComponentModel.DataAnnotations;

public class UpdateOrderDto
{
    [Required]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    public int? PaymentStatus { get; set; }

    public string? PaymentOrderRef { get; set; }

    public string? PaymentUrl { get; set; }

    [Required]
    public List<UpdateOrderItemDto> OrderItems { get; set; } = new List<UpdateOrderItemDto>();
}
