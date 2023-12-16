using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels   
{
    public partial class CustomerListViewModel : ObservableObject, INavigationAware
    {

        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public CustomerListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
        }


        [ObservableProperty]
        private string _nationalCode = "";

        [ObservableProperty]
        private string _serial = "";

        [ObservableProperty]
        private IEnumerable<PunListDto> _list;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }
        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            //List = await db.customerManager.(string.Empty, string.Empty);
        }

        [RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }
    }
}
