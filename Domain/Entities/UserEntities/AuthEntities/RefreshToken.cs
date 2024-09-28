using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntities.AuthEntities;

public class RefreshToken : BaseEntity
{
    [Required]
    public string? Token { get; set; }
    [Required]
    public string? JWTId { get; set; }
    [Required]
    public DateTimeOffset CreationDate { get; set; }
    [Required]
    public DateTimeOffset ExpiryDate { get; set; }
    [Required]
    public bool IsUsed { get; set; }
    [Required]
    public bool Invalidated { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
}
