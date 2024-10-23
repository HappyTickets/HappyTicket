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
        public string TeamAName { get; set; } = string.Empty;
        public string TeamALogo { get; set; } = string.Empty;
        public string TeamBName { get; set; } = string.Empty;
        public string TeamBLogo { get; set; } = string.Empty;
    }
}
