namespace Domain.Entities
{
    public class Sponsor : BaseEntity
    {
        public string Name { get; set; }
        public string? Logo { get; set; }
        public bool? IsHappySponsor { get; set; }

        public ICollection<ChampionSponsor>? ChampionSponsors { get; set; }
        public ICollection<TeamSponsor>? TeamSponsors { get; set; }

    }
}
