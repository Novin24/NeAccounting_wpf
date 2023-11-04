using DomainShared.ViewModels;

namespace NeAccounting.ViewModels
{
    public partial class PayViewModel : ObservableObject
    {
        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _autoSuggestBoxSuggestions = new List<SuggestBoxViewModel<Guid>>
        {

            new SuggestBoxViewModel<Guid>(){DisplayName = "احمد" ,Id =new Guid("ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName  = "رضا" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "ممد" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "رضا" ,Id =new Guid("ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "تقی" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "بهرام" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "شمس" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "مهدی" ,Id =new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da7e")},
            new SuggestBoxViewModel<Guid>(){DisplayName= "اسمال" ,Id =new Guid("ff24451a-96d3-4224-a349-d4a06ae2da7e") }
        };
    }
}
