using DomainShared.Constants;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using Microsoft.Identity.Client;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MaterailListViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        private bool _isreadonly = true;
        public MaterailListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
        }

        [ObservableProperty]
        private string _punName = "";

        [ObservableProperty]
        private string _serial = "";

        [ObservableProperty]
        private List<PunListDto> _list;

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
            List = await db.MaterialManager.GetMaterails(string.Empty, string.Empty);
        }

        [RelayCommand]
        private async Task OnSearchMaterial()
        {
            using UnitOfWork db = new();
            List = await db.MaterialManager.GetMaterails(PunName, Serial);
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

        //[RelayCommand]
        //private async Task OnRemoveMaterial(Guid parameter)
        //{
        //    var result = await _contentDialogService.ShowSimpleDialogAsync(
        //    new SimpleContentDialogCreateOptions()
        //    {
        //        Title = "آیا از حذف اطمینان دارید!!!",
        //        Content = Application.Current.Resources["DeleteDialogContent"],
        //        PrimaryButtonText = "بله",
        //        SecondaryButtonText = "خیر",
        //        CloseButtonText = "انصراف",
        //    });

        //    if (result == ContentDialogResult.Primary)
        //    {
        //        using UnitOfWork db = new();
        //        var isSuccess = await db.MaterialManager.DeleteAsync(parameter);
        //        if (!isSuccess)
        //        {
        //            _snackbarService.Show("کاربر گرامی", "خطا دراتصال به پایگاه داده!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //            return;
        //        }
        //        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        //        await OnSearchMaterial();
        //    }
        //}

        [RelayCommand]
        private async Task OnUpdateMaterial(Guid parameter)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var pun = List.First(t => t.Id == parameter);
            if (pun.IsServise)
            {
                Type? pageType = NameToPageTypeConverter.Convert("UpdateService");

                if (pageType == null)
                {
                    return;
                }
                var servise = _navigationService.GetNavigationControl();


                IEnumerable<SuggestBoxViewModel<Guid>> asuBox;

                using (UnitOfWork db = new())
                {
                    asuBox = await db.UnitManager.GetUnits(true);
                }

                var context = new UpdateServicePage(new UpdateServiceViewModel(_snackbarService, _navigationService)
                {
                    MaterialId = pun.Id,
                    Address = pun.Address,
                    Price = pun.LastSellPrice,
                    SrvicName = pun.MaterialName,
                    UnitId = pun.UnitId,
                    AsuBox = asuBox
                });

                servise.Navigate(pageType, context);
            }
            else
            {
                Type? pageType = NameToPageTypeConverter.Convert("UpdateMaterail");

                if (pageType == null)
                {
                    return;
                }
                var servise = _navigationService.GetNavigationControl();


                IEnumerable<SuggestBoxViewModel<Guid>> asuBox;

                using (UnitOfWork db = new())
                {
                    asuBox = await db.UnitManager.GetUnits(true);
                }

                var context = new UpdateMaterailPage(new UpdateMaterailViewModel(_snackbarService, _navigationService)
                {
                    MaterialId = pun.Id,
                    Serial = pun.Serial,
                    LastSellPrice = pun.LastSellPrice,
                    Address = pun.Address,
                    IsManufacturedGoods = pun.IsManufacturedGoods,
                    MaterialName = pun.MaterialName,
                    UnitId = pun.UnitId,
                    AsuBox = asuBox
                });

                servise.Navigate(pageType, context);
            }
        }

        [RelayCommand]
        private async Task OnActive(Guid id)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            await db.MaterialManager.ChangeStatus(id, true);
            await db.SaveChangesAsync();
            List = await db.MaterialManager.GetMaterails(string.Empty, string.Empty);
		}

		[RelayCommand]
        private async Task OnDeActive(Guid id)
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            await db.MaterialManager.ChangeStatus(id, false);
            await db.SaveChangesAsync();
            List = await db.MaterialManager.GetMaterails(string.Empty, string.Empty);
        }

    }
}
