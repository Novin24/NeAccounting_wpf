using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Helpers.Extention;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateWorkerViewModel : ObservableObject
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _dialogService;
        private bool _isreadonly = true;
        public CreateWorkerViewModel(INavigationService navigationService, ISnackbarService snackbarService, IContentDialogService dialogService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
            _dialogService = dialogService; 
            _isreadonly = NeAccountingConstants.ReadOnlyMode;
        }
        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _jobTitle;

        [ObservableProperty]
        private string _mobile;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private int _personalId = 0;

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private string _accountNumber;

        [ObservableProperty]
        private string _nationalCode;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private Shift _shift = Shift.ByMounth;

        [ObservableProperty]
        private long _salary = 0;

        [ObservableProperty]
        private long _overtimeSalary = 0;

        [ObservableProperty]
        private long _shiftSalary = 0;

        [ObservableProperty]
        private long _shiftovertimeSalary = 0;

        [ObservableProperty]
        private long _insurancePremium = 0;

        [ObservableProperty]
        private byte _dayInMonth = 0;


        [RelayCommand]
        private async Task OnCreate()
        {
            #region Validation
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کارگر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان شغل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            //if (string.IsNullOrEmpty(Address))
            //{
            //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            //    return;
            //}
            if (PersonalId <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("شماره پرسنلی", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByMounth && Salary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("دستمزد ماهانه", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByMounth && OvertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("دستمزد اضافه کاری ماهانه", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByHour && ShiftSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("دستمزد ", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByHour && ShiftovertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("دستمزد اضافه کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByMounth && InsurancePremium <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("مبلغ بیمه", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (Shift == Shift.ByMounth && DayInMonth <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMoreNumber("تعداد روز کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            
            if (Shift == Shift.ByMounth && DayInMonth > 31)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsLess("تعداد روز کاری", "سی و یک روز"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (string.IsNullOrEmpty(AccountNumber))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره حساب"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
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
            #endregion

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.WorkerManager.Create(
                       FullName,
                       NationalCode,
                       Mobile,
                       Address,
                       PersonalId,
                       AccountNumber,
                       Description,
                       JobTitle,
                       StartDate,
                       Shift,
                       Salary,
                       OvertimeSalary,
                       ShiftSalary,
                       ShiftovertimeSalary,
                       InsurancePremium,
                       DayInMonth);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

            Type? pageType = NameToPageTypeConverter.Convert("WorkersList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
