using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities;

public class Championship : SoftDeletableEntity<long>
{
    [Required]
    public string Name { get; set; }

    public string? Logo { get; set; }


    public virtual ICollection<ChampionSponsor>? ChampionSponsors { get; set; }

    public virtual ICollection<Match>? Matches { get; set; }
}
