using Shared.DTOs.Champion;

namespace Shared.DTOs
{
    public class ChampionSponsorDto
    {
        public Guid Id { get; set; }
        public Guid SponsorId { get; set; }
        public Guid ChampionId { get; set; }

        public ChampionDto Champion { get; set; }
        public SponsorDto Sponsor { get; set; }
    }
}
