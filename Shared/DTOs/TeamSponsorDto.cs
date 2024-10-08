using Shared.DTOs.Champion;
using Shared.DTOs.Team;

namespace Shared.DTOs
{
    public class TeamSponsorDto
    {
        public Guid Id { get; set; }
        public Guid SponsorId { get; set; }
        public Guid TeamId { get; set; }

        public TeamDto Team { get; set; }
        public SponsorDto Sponsor { get; set; }

    }
}
