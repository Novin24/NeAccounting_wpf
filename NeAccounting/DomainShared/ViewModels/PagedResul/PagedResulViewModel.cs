namespace DomainShared.ViewModels.PagedResul
{
    public class PagedResulViewModel<T>
    {
        private IEnumerable<T> _items;
        public int TotalCount { get; set; }
        public int PageCount
        {
            get
            {
                int pageCount = TotalCount / RowInPage;
                if (TotalCount % RowInPage != 0)
                {
                    pageCount++;
                }
                return pageCount;
            }
        }
        public int RowInPage { get; set; }
        public IEnumerable<T> Items
        {
            get { return _items ??= new List<T>(); }
            set { _items = value; }
        }
        public PagedResulViewModel(int totalCount, int rowInPage, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            RowInPage = rowInPage;
            Items = items;
        }


    }
}
