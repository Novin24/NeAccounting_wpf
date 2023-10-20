namespace NeAccounting.ViewModels
{
    public partial class PayViewModel : ObservableObject
    {
        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<int>> _autoSuggestBoxSuggestions = new List<SuggestBoxViewModel<int>>
        {

            new SuggestBoxViewModel<int>(){DisplayName = "Phoebe" ,Id = 1},
            new SuggestBoxViewModel<int>(){DisplayName  = "Lucas" ,Id = 2},
            new SuggestBoxViewModel<int>(){DisplayName= "Carl" ,Id = 3},
            new SuggestBoxViewModel<int>(){DisplayName= "Marissa" ,Id =4 },
            new SuggestBoxViewModel<int>(){DisplayName= "Brandon" ,Id = 5},
            new SuggestBoxViewModel<int>(){DisplayName= "Antoine" ,Id = 6},
            new SuggestBoxViewModel<int>(){DisplayName= "Arielle" ,Id = 7},
            new SuggestBoxViewModel<int>(){DisplayName= "Jamie" ,Id = 8},
            new SuggestBoxViewModel<int>(){DisplayName= "Alexzander" ,Id =9 }
        };
    }
}
