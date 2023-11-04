using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class DefinitoinOfPunViewModel : ObservableObject, INavigationAware
    {


        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _autoSuggestBoxSuggestions;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
           await InitializeViewModel();
        }


        private async Task InitializeViewModel()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                _autoSuggestBoxSuggestions = await db.customerManager.GetDisplayUser();
            }
        }
    }
}
