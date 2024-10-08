namespace Domain.Entities
{
    public class TeamSponsor: BaseEntity
    {
        public Guid SponsorId { get; set; }
        public Guid TeamId { get; set; }

        public Team Team { get; set; }
        public Sponsor Sponsor { get; set; }
    }
}
