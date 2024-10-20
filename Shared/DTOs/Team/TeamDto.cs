namespace Shared.DTOs.Team
{
    public class TeamDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Logo { get; set; }
        public ICollection<long>? TicketIds { get; set; }
        public ICollection<long>? UserFavoriteTeamIds { get; set; }
        public IEnumerable<TeamSponsorDto>? TeamSponsors { get; set; }

    }
}
