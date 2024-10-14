using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class TeamSponsor : BaseEntity<long>
{
    [Required]
    public long SponsorId { get; set; }

    [ForeignKey(nameof(SponsorId))]
    public virtual Sponsor Sponsor { get; set; }

    [Required]
    public long TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public virtual Team Team { get; set; }
}
