using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Team : SoftDeletableEntity<long>
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<TeamSponsor>? TeamSponsors { get; set; }

    public virtual ICollection<MatchTeam>? MatchTeams { get; set; }
}
