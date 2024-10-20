namespace Shared.Common.General
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public long TotalItems { get; set; }
        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => PageIndex < TotalPages - 1;

        public PaginatedList(IEnumerable<T> items, long count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalItems = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static PaginatedList<T> Create(IEnumerable<T> items, long count, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
