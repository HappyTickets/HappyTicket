using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MatchDtos
{
    public class GetPaginatedMatchesDto : BaseMatch
    {
        public bool IsOver { get; set; }
    }
}
