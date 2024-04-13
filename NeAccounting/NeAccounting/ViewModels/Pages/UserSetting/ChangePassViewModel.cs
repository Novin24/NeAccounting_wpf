using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class ChangePassViewModel : ObservableObject
    {

        private bool _isInitialized = false;
        private readonly INavigationService _navigationService;
        private readonly ISnackbarService _snackbarService;
        public ChangePassViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _currentPass;

        [ObservableProperty]
        private string _newPass;

        [ObservableProperty]
        private string _reNewPass;

        [RelayCommand]
        private async Task OnChangePass()
        {
            if (string.IsNullOrEmpty(CurrentPass))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("رمز عبور فعلی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(NewPass))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("رمز عبور جدید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(ReNewPass))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تکرار رمز عبور جدید"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (NewPass.Length < 8)
            {
                _snackbarService.Show("خطا", "رمز عبور جدید باید حداقل 8 کارکتر باشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (NewPass != ReNewPass)
            {
                _snackbarService.Show("خطا", "رمز عبور و تکرار رمز متفاوت است!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using (BaseUnitOfWork db = new())
            {
                var (i, e) = await db.UserRepository.ChangePass(CurrentPass, NewPass);
                if (!i)
                {
                    _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }

            _snackbarService.Show("کاربر گرامی", "تغییر پسورد با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
