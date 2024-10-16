using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities;

public class Sponsor : SoftDeletableEntity<long>
{
    [Required]
    public string Name { get; set; }

    public string? Logo { get; set; }

    public bool? IsHappySponsor { get; set; }

    public virtual ICollection<ChampionSponsor>? ChampionSponsors { get; set; }

    public virtual ICollection<TeamSponsor>? TeamSponsors { get; set; }
}
