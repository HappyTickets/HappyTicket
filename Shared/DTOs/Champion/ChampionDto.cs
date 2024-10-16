namespace Shared.DTOs.Champion
{
    public class ChampionDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public IEnumerable<ChampionSponsorDto>? ChampionSponsors { get; set; }
    }
}
