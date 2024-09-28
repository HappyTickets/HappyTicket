using Shared.Common.General;

namespace Shared.Common.Interfaces
{
    public interface IPaginationSearchModel : IDisposable
    {

        PaginationSearchModel GetPaginationModel(int Pageindex = 0, int Pagesize = 15, bool PaginationOff = false);
        PaginationSearchModel GetPaginationSearchModel(int Pageindex, int Pagesize, string SearchKey,
            string SearchIn, DateTime? FromDate, DateTime? ToDate, string OrderBy, bool paginationOff);
        PaginationSearchModel GetPaginationModel(bool off);
    }
}
