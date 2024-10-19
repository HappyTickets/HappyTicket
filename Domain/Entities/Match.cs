using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Match : SoftDeletableEntity<long>
{
    public int MaxPerUser { get; set; }

    [Required]
    public DateTime? EventDate { get; set; }

    public TimeSpan? EventTime { get; set; }

    [Required]
    public long StadiumId { get; set; }

    [ForeignKey(nameof(StadiumId))]
    public virtual Stadium Stadium { get; set; }

    [Required]
    public long ChampionId { get; set; }

    [ForeignKey(nameof(ChampionId))]
    public virtual Championship Champion { get; set; }
    public virtual ICollection<MatchTeam>? MatchTeams { get; set; } = new List<MatchTeam>();

}
