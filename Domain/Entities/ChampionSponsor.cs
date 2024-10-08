namespace Domain.Entities
{
    public class ChampionSponsor : BaseEntity
    {
        public Guid SponsorId { get; set; }
        public Guid ChampionId { get; set; }

        public Champion Champion { get; set; }
        public Sponsor Sponsor { get; set; }
    }
}
