using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Helpers.Extention;
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
        private int? _personalId;

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private string _accountNumber;

        [ObservableProperty]
        private string _nationalCode;

        [ObservableProperty]
        private string _description;

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
        private long? _salary;

        [ObservableProperty]
        private long? _overtimeSalary;

        [ObservableProperty]
        private long? _insurancePremium;

        [ObservableProperty]
        private byte? _dayInMonth;

        [ObservableProperty]
        private bool _shiftByMonth;

        [ObservableProperty]
        private bool _shiftByHour = true;


        [ObservableProperty]
        private long? _shiftSalary;

        [ObservableProperty]
        private long? _shiftovertimeSalary;


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
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کارگر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان شغل"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (PersonalId == null || PersonalId <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("شماره پرسنلی", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByHour && (ShiftSalary == null || ShiftSalary <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد ", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByHour && (ShiftovertimeSalary == null || ShiftovertimeSalary <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && (OvertimeSalary == null || OvertimeSalary <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && (Salary == null || Salary <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && (InsurancePremium == null || InsurancePremium <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("مبلغ بیمه", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (WorkerShift == Shift.ByMounth && (DayInMonth == null || DayInMonth <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("تعداد روز کاری", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (string.IsNullOrEmpty(AccountNumber))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره حساب"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
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
                       PersonalId.Value,
                       AccountNumber,
                       Description,
                       JobTitle,
                       Status,
                       WorkerShift,
                       Salary.Value,
                       OvertimeSalary.Value,
                       ShiftSalary.Value,
                       ShiftovertimeSalary.Value,
                       InsurancePremium.Value,
                       DayInMonth.Value);

            if (!isSuccess)
            {
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            await db.SaveChangesAsync();


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("WorkersList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
