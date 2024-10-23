using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MatchDtos
{
    public class UpdateMatchDto
    {
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public long TeamAId { get; set; }
        public long TeamBId { get; set; }
        public long StadiumId { get; set; }
        public long ChampionId { get; set; }
    }
}
