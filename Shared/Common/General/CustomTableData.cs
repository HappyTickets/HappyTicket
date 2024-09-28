namespace Shared.Common.General
{
    public class CustomTableData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
        public CustomTableData(List<T> values, int totalItem)
        {
            Items = values;
            TotalItems = totalItem;
        }
    }
}
