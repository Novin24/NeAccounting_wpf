namespace DomainShared.ViewModels.PagedResul
{
    public class PagedResulViewModel<T>
    {
        private IEnumerable<T> _items;
        private int currentPage;
        public int TotalCount { get; set; }
        public int RowInPage { get; set; }
        public int PageCount
        {
            get
            {
                int pageCount = TotalCount / RowInPage;
                if (TotalCount % RowInPage != 0)
                {
                    pageCount++;
                }
                return pageCount == 0 ? 1 : pageCount;
            }
        }
        public int CurrentPage
        {
            get => currentPage != 0 ? currentPage : 1;
            set => currentPage = value;
        }
        public IEnumerable<T> Items
        {
            get { return _items ??= new List<T>(); }
            set { _items = value; }
        }

        public PagedResulViewModel(int totalCount, int rowInPage, int currentPage, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            RowInPage = rowInPage;
            Items = items;
            CurrentPage = currentPage;
        }
    }
}
