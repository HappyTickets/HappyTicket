using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class MatchTeam : SoftDeletableEntity<long>
{
    [Required]
    public long MatchId { get; set; }

    [ForeignKey(nameof(MatchId))]
    public virtual Match Match { get; set; }

    [Required]
    public long TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public virtual Team Team { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<SelectedTeam>? UserSelectedTeams { get; set; }
}
