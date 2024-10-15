using Shared.DTOs.Champion;
using Shared.DTOs.TicketDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MatchDtos
{
    public class MatchCreateOrUpdateDto
    {
        public long Id { get; set; }
        [Required]
        public DateTime EventDate { get; set; }

        public TimeSpan? EventTime { get; set; }

        [Required]
        public long StadiumId { get; set; }

        [Required]
        public long ChampionId { get; set; }
    }


    public class MatchDto
    {
        public int MaxPerUser { get; set; } = 1;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }

        public long Id { get; set; }

        public long StadiumId { get; set; }
        public StadiumDto? Stadium { get; set; } = null;
        public long ChampionId { get; set; }
        public ChampionDto? Champion { get; set; }


        public bool IsActive { get; set; }
        public bool IsOver { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
