using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MatchDtos
{
    public class UpdateMatchDto : BaseMatch
    {
        public long MatchId { get; set; }
        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public string TeamAName { get; set; } = string.Empty;
        public string TeamBName { get; set; } = string.Empty;
    }
}
