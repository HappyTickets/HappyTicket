namespace Shared.DTOs.Champion
{
    public class ChampionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public IEnumerable<ChampionSponsorDto>? ChampionSponsors { get; set; }
    }
}
