using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class UpdateMaterailViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _asuBox;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                await InitializeViewModel();
        }


        private async Task InitializeViewModel()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                await db.materialManager.TolistAsync(t => t.Serial == "");
            }
            _isInitialized = true;
        }

    }
}
