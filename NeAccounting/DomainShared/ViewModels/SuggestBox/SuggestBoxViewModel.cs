namespace DomainShared.ViewModels
{
    public class SuggestBoxViewModel<T>
    {
        public T Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class SuggestBoxViewModel<T,S>
    {
        public T Id { get; set; }
        public string DisplayName { get; set; }
        public S UniqNumber { get; set; }
    }

    public class PersonnerlSuggestBoxViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int PersonnelId { get; set; }
    }

}