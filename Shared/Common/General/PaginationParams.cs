using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.General
{
    public class PaginationParams
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public string order { get; set; }
    }
}
