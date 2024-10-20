using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MatchDtos
{
    public class GetAllMatchesDto : BaseMatch
    {
        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public bool IsOver { get; set; }
        public bool? HasTickets { get; set; } = false;
    }
}
