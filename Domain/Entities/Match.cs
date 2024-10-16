using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class Match : SoftDeletableEntity<long>
{
    [Required]
    public DateTime EventDate { get; set; } // Non-nullable if it's required

    public TimeSpan? EventTime { get; set; } // Keep nullable if optional

    [Required]
    public long StadiumId { get; set; }

    [ForeignKey(nameof(StadiumId))]
    public virtual Stadium Stadium { get; set; }

    [Required]
    public long ChampionshipId { get; set; } // Use 'ChampionshipId' for consistency

    [ForeignKey(nameof(ChampionshipId))]
    public virtual Championship Championship { get; set; } // Use 'Championship' for clarity

    public long HomeTeamId { get; set; }

    [ForeignKey(nameof(HomeTeamId))]
    public virtual MatchTeam HomeTeam { get; set; }

    public long AwayTeamId { get; set; }

    [ForeignKey(nameof(AwayTeamId))]
    public virtual MatchTeam AwayTeam { get; set; }
}
