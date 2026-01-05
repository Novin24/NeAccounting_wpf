using System.Collections.ObjectModel;
using System.Windows.Media;
using DomainShared.Errore;
using DomainShared.ViewModels.Menu;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Helpers.Extention;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateUserViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService dialogService) : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly IContentDialogService _dialogService = dialogService;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _surName;

        [ObservableProperty]
        private string _nationalCode;

        [ObservableProperty]
        private string _mobile;

        [ObservableProperty]
        private ObservableCollection<MenuListDto> _permissions;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            var db = new BaseUnitOfWork();
            Permissions = new ObservableCollection<MenuListDto>(await db.MenuRepository.GetMenuList());
        }

        [RelayCommand]
        private async Task OnCreateUser()
        {

            if (string.IsNullOrEmpty(UserName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کاربری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Name))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کاربر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(SurName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام خانوادگی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(NationalCode))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("کد ملی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (!string.IsNullOrEmpty(NationalCode))
            {
                if (!NationalCode.ValidNationalCode(_snackbarService))
                {
                    var result = await _dialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
                    {
                        Title = "کد ملی نامعتبر !!!",
                        Content = new TextBlock() { Text = "آیا ادامه میدهید ؟؟؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                        PrimaryButtonText = "بله",
                        SecondaryButtonText = "خیر",
                        CloseButtonText = "انصراف",
                    });
                    if (result != ContentDialogResult.Primary)
                    {
                        return;
                    }
                }
            }
            else
            {
                NationalCode = string.Empty;
            }

            Mobile = Mobile.Trim();
            if (!Mobile.ValidMobileNumber())
            {
                var result = await _dialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
                {
                    Title = "موبایل نامعتبر !!!",
                    Content = new TextBlock() { Text = "آیا ادامه میدهید ؟؟؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                    PrimaryButtonText = "بله",
                    SecondaryButtonText = "خیر",
                    CloseButtonText = "انصراف",
                });
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }
            }

 
            using (BaseUnitOfWork db = new())
            {
                var (error, isSuccess) = await db.UserRepository.CreateUser(UserName, Name, SurName, NationalCode, Mobile,
                    [.. Permissions.SelectMany(GetSelectedIds).Distinct()]);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
            }


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("UserList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

        [RelayCommand]
        private void OnPermissionChecked(MenuListDto obj)
        {
            if (obj == null || obj.IsChecked == null) return;
            obj.SetChecked(obj.IsChecked.Value);
            obj.UpdateParentStatus();
        }

        private static IEnumerable<Guid> GetSelectedIds(MenuListDto menu)
        {
            if (menu == null)
                yield break;

            if (menu.IsChecked is true or null)
                yield return menu.Id;

            if (menu.Children == null)
                yield break;

            foreach (var child in menu.Children)
            {
                foreach (var id in GetSelectedIds(child))
                    yield return id;
            }
        }


    }
}
