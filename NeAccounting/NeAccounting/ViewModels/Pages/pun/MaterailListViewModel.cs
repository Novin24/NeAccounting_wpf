using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MaterailListViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;


        public MaterailListViewModel(INavigationService navigationService, IContentDialogService contentDialogService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
        }

        [ObservableProperty]
        private string _punName = "";

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
            List = await db.materialManager.GetMaterails(string.Empty, string.Empty);
        }

        [RelayCommand]
        private async Task OnSearchMaterial()
        {
            using UnitOfWork db = new();
            List = await db.materialManager.GetMaterails(PunName, Serial);
        }

        [RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
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

        [RelayCommand]
        private async Task OnRemoveMaterial(int parameter)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از حذف اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {

            }
        }

        [RelayCommand]
        private void OnUpdateMaterial(int parameter)
        {

        }
    }
}
