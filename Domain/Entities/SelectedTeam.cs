using Domain.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class SelectedTeam : BaseEntity<long>
{
    [Required]
    public long MatchTeamId { get; set; }

    [ForeignKey(nameof(MatchTeamId))]
    public virtual MatchTeam MatchTeam { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
}
