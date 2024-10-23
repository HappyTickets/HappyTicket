using Shared.DTOs.Champion;
using Shared.DTOs.Team;
using Shared.DTOs.TicketDTOs;

namespace Shared.DTOs.MatchDtos
{
    public class MatchDto
    {
        public int MaxPerUser { get; set; } = 1;
        public long Id { get; set; }
        public TeamDto? TeamA { get; set; }
        public TeamDto? TeamB { get; set; }
        public StadiumDto? Stadium { get; set; } = null;
        public ChampionDto? Champion { get; set; }
        public bool? IsActive { get; set; }
        public bool IsOver { get; set; }
        public List<TicketDto> Tickets { get; set; }
        public bool? HasTickets { get; set; } = false;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public long TeamAId { get; set; }
        public long TeamBId { get; set; }
        public long StadiumId { get; set; }
        public long ChampionId { get; set; }
    }
}
