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
        public UpdateWorkerViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
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
        private uint _salary = 0;

        [ObservableProperty]
        private uint _overtimeSalary = 0;

        [ObservableProperty]
        private uint _insurancePremium = 0;

        [ObservableProperty]
        private byte _dayInMonth = 0;

        [ObservableProperty]
        private bool _shiftByMonth;

        [ObservableProperty]
        private bool _shiftByHour;


        [ObservableProperty]
        private uint _shiftSalary;

        [ObservableProperty]
        private uint _shiftovertimeSalary;


        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {

        }

        [RelayCommand]
        private async Task OnUpdate()
        {

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

            if (WorkerShift == Shift.ByHour &&  ShiftSalary <= 0)
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
                return;
            }

            using UnitOfWork db = new();
            var (error, isSuccess) = await db.workerManager.Update(
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
