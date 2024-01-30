namespace DomainShared.ViewModels.PagedResul
{
    public class PagedResulViewModel<T>
    {
        private IEnumerable<T> _items;
        public string TotalCount { get; set; }
        public IEnumerable<T> Items
        {
            get { return _items ??= new List<T>(); }
            set { _items = value; }
        }
        public PagedResulViewModel(string totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }


    }
}
