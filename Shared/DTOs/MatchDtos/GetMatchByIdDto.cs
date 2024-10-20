using Shared.DTOs.Champion;
using Shared.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MatchDtos
{
    public class GetMatchByIdDto
    {
        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public StadiumDto? Stadium { get; set; } = null;
        public ChampionDto? Champion { get; set; }
        public bool IsOver { get; set; }
        public bool? HasTickets { get; set; } = false;
        public TeamDto? TeamA { get; set; }
        public TeamDto? TeamB { get; set; }
    }
}
