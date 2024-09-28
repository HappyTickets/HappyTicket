using Domain.Entities.CartEntity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntities;

public class ApplicationUser : IdentityUser
{
    public Guid CartId { get; set; }
    [ForeignKey(nameof(CartId))]
    public virtual Cart? Cart { get; set; }
    public virtual ICollection<UserFavoriteTeam>? FavoriteTeams { get; set; }

    public int SoftDeleteCount { get; set; } = 0;
    public BaseEntityStatus? BaseEntityStatus { get; set; } = null;

    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
