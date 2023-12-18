using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MaterailListViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        public MaterailListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
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
                using UnitOfWork db = new();
                var isSuccess = await db.materialManager.DeleteAsync(parameter);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", "خطا دراتصال به پایگاه داده!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

                await OnSearchMaterial();
            }
        }

        [RelayCommand]
        private async Task OnUpdateMaterial(int parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateMaterail");

            if (pageType == null)
            {
                return;
            }
            var servise = _navigationService.GetNavigationControl();

            var pun = List.First(t => t.Id == parameter);

            IEnumerable<SuggestBoxViewModel<int>> asuBox;

            using (UnitOfWork db = new())
            {
                asuBox = await db.unitManager.GetUnits();
            }

            var context = new UpdateMaterailPage(new Pages.UpdateMaterailViewModel(_snackbarService, _navigationService)
            {
                MaterialId = pun.Id,
                Serial = pun.Serial,
                LastSellPrice = pun.LastPrice,
                Address = pun.Address,
                IsManufacturedGoods = pun.IsManufacturedGoods,
                MaterialName = pun.MaterialName,
                UnitId = pun.UnitId,
                AsuBox = asuBox
            });

            servise.Navigate(pageType, context);
        }
    }
}
