using DomainShared.ViewModels;

namespace NeAccounting.ViewModels
{
    public partial class RecViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _auSuBox;

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            AuSuBox = new List<SuggestBoxViewModel<Guid>>
                {
                    new SuggestBoxViewModel<Guid>(){DisplayName = "احمد" ,Id =
                    new Guid("ff24451a-96d3-4224-a349-d4a06ae2da71")},

                    new SuggestBoxViewModel<Guid>(){DisplayName  = "رضا" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da72")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "ممد" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da73")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "محسن" ,Id =
                    new Guid("ff24451a-96d3-4224-a349-d4a06ae2da74")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "تقی" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da75")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "بهرام" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da76")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "شمس" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da77")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "مهدی" ,Id =
                    new Guid( "ff24451a-96d3-4224-a349-d4a06ae2da78")},

                    new SuggestBoxViewModel<Guid>(){DisplayName= "اسمال" ,Id =
                    new Guid("ff24451a-96d3-4224-a349-d4a06ae2da79") }
                };
        }
    }
}
