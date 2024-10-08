using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntities;

public class UserFavoriteTeam : BaseEntity
{
    public Guid MatchId { get; set; }
    [ForeignKey(nameof(MatchId))]
    public virtual Match? Match { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
    public Guid TeamId { get; set; }
    [ForeignKey(nameof(TeamId))]
    public virtual Team? Team { get; set; }
}
