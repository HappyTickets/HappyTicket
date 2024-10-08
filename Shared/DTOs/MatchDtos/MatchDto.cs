using Shared.DTOs.Champion;
using Shared.DTOs.Team;
using Shared.DTOs.TicketDTOs;

namespace Shared.DTOs.MatchDtos
{
    public class MatchDto
    {
        public int MaxPerUser { get; set; } = 1;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }

        public Guid Id { get; set; }
        public Guid TeamAId { get; set; }
        public TeamDto? TeamA { get; set; }
        public Guid TeamBId { get; set; }
        public TeamDto? TeamB { get; set; }
        public Guid StadiumId { get; set; }
        public StadiumDto? Stadium { get; set; } = null;
        public Guid ChampionId { get; set; }
        public ChampionDto? Champion { get; set; }


        public bool? IsActive { get; set; }
        public bool IsOver { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
