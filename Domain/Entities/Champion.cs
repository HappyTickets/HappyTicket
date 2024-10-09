namespace Domain.Entities
{
    public class Champion : BaseEntity
    {
        public string Name { get; set; }
        public string? Logo { get; set; }

        public ICollection<ChampionSponsor>? ChampionSponsors { get; set; }
    }
}
