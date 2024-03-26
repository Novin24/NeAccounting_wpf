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
    public partial class UpdateWorkerViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _dialogService;

        public UpdateWorkerViewModel(INavigationService navigationService, ISnackbarService snackbarService, IContentDialogService dialogService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
            _dialogService = dialogService;
        }
        [ObservableProperty]
        private int _id;

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
        private string? _description;

        public Shift WorkerShift
        {
            get
            {
                return ShiftByMonth ? Shift.ByMounth : Shift.ByHour;
            }
            set
            {
                ShiftByHour = value == Shift.ByHour;
                ShiftByMonth = value == Shift.ByMounth;
            }
        }

        public Status Status { get { return (Status)StatusByte; } set { StatusByte = (byte)value; } }

        [ObservableProperty]
        private byte _statusByte;

        [ObservableProperty]
        private long _salary = 0;

        [ObservableProperty]
        private long _overtimeSalary = 0;

        [ObservableProperty]
        private long _insurancePremium = 0;

        [ObservableProperty]
        private byte _dayInMonth = 0;

        [ObservableProperty]
        private bool _shiftByMonth;

        [ObservableProperty]
        private bool _shiftByHour;


        [ObservableProperty]
        private long _shiftSalary;

        [ObservableProperty]
        private long _shiftovertimeSalary;


        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {

        }

        [RelayCommand]
        private async Task OnUpdate()
        {
            #region validation

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
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (PersonalId == null || PersonalId <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("شماره پرسنلی", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByHour && ShiftSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد ", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByHour && ShiftovertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && OvertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && Salary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && InsurancePremium <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("مبلغ بیمه", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && DayInMonth <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("تعداد روز کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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

            using UnitOfWork db = new();
            var (error, isSuccess) = await db.WorkerManager.Update(
                       Id,
                       FullName,
                       NationalCode,
                       Mobile,
                       Address,
                       StartDate,
                       PersonalId,
                       AccountNumber,
                       Description,
                       JobTitle,
                       Status,
                       WorkerShift,
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
            await db.SaveChangesAsync();


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
