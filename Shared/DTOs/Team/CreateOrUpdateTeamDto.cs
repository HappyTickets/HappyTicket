namespace Shared.DTOs.Team
{
    public class CreateOrUpdateTeamDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string? Description { get; set; }

        public IEnumerable<Guid>? SponsorsIds { get; set; }
    }
}
