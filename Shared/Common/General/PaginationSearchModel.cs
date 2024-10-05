using Shared.Common.Interfaces;

namespace Shared.Common.General
{
    public class PaginationSearchModel : IPaginationSearchModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool PaginationOff { get; set; } = true;
        //Search Parameters
        public string? SearchKey { get; set; }
        public string? SearchIn { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; }


        public PaginationSearchModel GetPaginationModel(int index = 0, int size = 15, bool off = false)
        {
            return new PaginationSearchModel
            {
                PageSize = size,
                PageIndex = index,
                PaginationOff = off,
            };
        }

        public PaginationSearchModel GetPaginationModel(bool off = true)
        {
            return new PaginationSearchModel
            {
                PaginationOff = off,
            };
        }

        public PaginationSearchModel GetPaginationSearchModel(int Pageindex = 0, int Pagesize = 15, string searchKey = null,
             string searchIn = null, DateTime? fromDate = null, DateTime? toDate = null, string orderBy = null, bool paginationOff = false)
        {
            return new PaginationSearchModel
            {
                PageSize = Pagesize,
                PageIndex = Pageindex,
                SearchKey = searchKey,
                SearchIn = searchIn,
                FromDate = fromDate,
                ToDate = toDate,
                OrderBy = orderBy,
                PaginationOff = paginationOff
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
