using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Windows;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class HotCreateMaterailViewModel(ISnackbarService snackbarService) : ObservableObject
    {

        private readonly ISnackbarService _snackbarService = snackbarService;
        private bool _isreadonly = NeAccountingConstants.ReadOnlyMode;
        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<Guid>> _asuBox;

        [ObservableProperty]
        private string _materialName;

        [ObservableProperty]
        private string _serial;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private long _lastSellPrice = 0;

        [ObservableProperty]
        private Guid? _unitId;

        [ObservableProperty]
        private bool _isManufacturedGoods = false;

        [RelayCommand]
        private async Task OnInitializeViewModel()
        {
            using (UnitOfWork db = new())
            {
                AsuBox = await db.UnitManager.GetUnits();
            }

            if (AsuBox.Any())
            {
                UnitId = AsuBox.First().Id;
            }

        }

        [RelayCommand]
        private async Task OnCreateMat(CreateMaterialWindow window)
        {

            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(MaterialName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            //if (string.IsNullOrEmpty(Serial))
            //{
            //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سریال کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            //    return;
            //}
            if (UnitId == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("واحد کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            //if (string.IsNullOrEmpty(Address))
            //{
            //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مکان فیزیکی کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            //    return;
            //}

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.MaterialManager.CreateMaterial(MaterialName, UnitId.Value, false, LastSellPrice, Serial, Address, IsManufacturedGoods);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            window.Visibility = Visibility.Hidden;
        }
    }
}
