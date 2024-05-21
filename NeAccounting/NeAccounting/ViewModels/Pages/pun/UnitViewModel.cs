using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels.unit;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UnitViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly ISnackbarService _snackbarService;
        private bool _isreadonly = true;

        public UnitViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
            _isreadonly = NeAccountingConstants.ReadOnlyMode;

        }

        [ObservableProperty]
        private Guid? _unitId;

        [ObservableProperty]
        private string _unitName;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private List<UnitListDto> _list;

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
            using (UnitOfWork db = new())
            {
                List = await db.UnitManager.GetUnitList();
            }

            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OnCreateUnit()
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(UnitName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام واحد"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using UnitOfWork db = new();
            if (UnitId != null)
            {
                var (error, isSuccess) = await db.UnitManager.UpdateUnit(UnitId.Value, UnitName, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                UnitId = null;
                UnitName = string.Empty;
                Description = string.Empty;

                _snackbarService.Show("کاربر گرامی", "عملیات ویرایش با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            }
            else
            {
                var (error, isSuccess) = await db.UnitManager.CreateUnit(UnitName, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                _snackbarService.Show("کاربر گرامی", "عملیات ثبت با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            }
            List = await db.UnitManager.GetUnitList();
            UnitName = string.Empty;
            Description = string.Empty;
        }

        [RelayCommand]
        private async Task OnActive(Guid id)
        {
            using UnitOfWork db = new();
            await db.UnitManager.ChangeStatus(id, true);
            await db.SaveChangesAsync();
            List = await db.UnitManager.GetUnitList();
        }

        [RelayCommand]
        private async Task OnDeActive(Guid id)
        {
            using UnitOfWork db = new();
            await db.UnitManager.ChangeStatus(id, false);
            await db.SaveChangesAsync();
            List = await db.UnitManager.GetUnitList();
        }
    }
}
