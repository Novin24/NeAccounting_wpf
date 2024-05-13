using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Helpers.Extention;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class UpdateCustomerViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService dialogService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly IContentDialogService _dialogService = dialogService;
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


        public Guid Id { get; set; }

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
        private bool _haveChequeCredit = false;

        [ObservableProperty]
        private bool _havePromissoryNote = false;

        [ObservableProperty]
        private long? _promissoryNote = 0;

        [ObservableProperty]
        private long? _cashCredit = 0;

        [ObservableProperty]
        private long? _chequeCredit = 0;

        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            #region validation
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            // محمد تقی در تاریخ 23 اردیبهشت 1403 گفت کملی الزامی نباشد
            //if (string.IsNullOrEmpty(NationalCode))
            //{
            //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("کد ملی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            //    return;
            //}
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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

            PromissoryNote ??= 0;
            CashCredit ??= 0;
            #endregion

            #region CreateCustomer
            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.CustomerManager.UpdateCustomer(Id, FullName, Mobile, CashCredit.Value,
                    PromissoryNote.Value, NationalCode, Address, (CustomerType)CusType, HavePromissoryNote, HaveCashCredit, Buyer, Seller);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();
            }
            #endregion

            #region NvigateToList
            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("CustomerList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
            #endregion
        }
    }
}
