using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class ChampionSponsor : SoftDeletableEntity<long>
{
    [Required]
    public long SponsorId { get; set; }

    [ForeignKey(nameof(SponsorId))]
    public virtual Sponsor Sponsor { get; set; }

    [Required]
    public long ChampionId { get; set; }

    [ForeignKey(nameof(ChampionId))]
    public virtual Championship Champion { get; set; }
}
