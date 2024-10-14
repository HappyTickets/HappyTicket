using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class Match : SoftDeletableEntity<long>
{
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

    //public virtual MatchTeam HomeTeam { get; set; }

    //public virtual MatchTeam AwayTeam { get; set; }
}
