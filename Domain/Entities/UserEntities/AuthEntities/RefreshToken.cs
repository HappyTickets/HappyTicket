using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntities.AuthEntities;

public class RefreshToken : BaseEntity<long>
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
    public long UserId { get; set; }
    [Required]
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
}
