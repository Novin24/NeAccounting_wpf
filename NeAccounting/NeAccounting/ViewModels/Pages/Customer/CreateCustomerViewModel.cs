using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateCustomerViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISnackbarService _snackbarService = snackbarService;
        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _nationalCode;

        [ObservableProperty]
        private string _mobile;

        [ObservableProperty]
        private byte _cusType = 0;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private bool _buyer = true;

        [ObservableProperty]
        private bool _seller = true;

        [ObservableProperty]
        private bool _haveCashCredit = false;

        [ObservableProperty]
        private bool _havePromissoryNote = false;

        [ObservableProperty]
        private long? _promissoryNote = 0;

        [ObservableProperty]
        private long? _cashCredit = 0;



        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            if (string.IsNullOrEmpty(FullName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (HavePromissoryNote && (PromissoryNote == null || PromissoryNote == 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اعتبار سفته"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (HaveCashCredit && (CashCredit == null || CashCredit == 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اعتبار نقدی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }


            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.CustomerManager.CreateCustomer(FullName, Mobile, CashCredit.Value, PromissoryNote.Value, NationalCode, Address, (CustomerType)CusType, HavePromissoryNote, HaveCashCredit, Buyer, Seller);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("CustomerList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }

    }
}
