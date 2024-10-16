using Domain.Entities.UserEntities.AuthEntities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntities;

public class ApplicationUser : IdentityUser<long>
{
    public long CartId { get; set; }
    [ForeignKey(nameof(CartId))]
    public virtual Cart? Cart { get; set; }
    public virtual ICollection<SelectedTeam>? SelectedTeams { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

    public int SoftDeleteCount { get; set; } = 0;
    public BaseEntityStatus? BaseEntityStatus { get; set; } = null;

    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
