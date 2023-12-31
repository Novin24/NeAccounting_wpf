using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Helpers.Extention;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateWorkerViewModel : ObservableObject
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        public CreateWorkerViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _snackbarService = snackbarService;
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
        private uint _salary = 0;

        [ObservableProperty]
        private uint _overtimeSalary = 0;

        [ObservableProperty]
        private uint _shiftSalary = 0;

        [ObservableProperty]
        private uint _shiftovertimeSalary = 0;

        [ObservableProperty]
        private uint _insurancePremium = 0;

        [ObservableProperty]
        private byte _dayInMonth = 0;


        [RelayCommand]
        private async Task OnCreate()
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

            if (Salary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد ماهانه", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (OvertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری ماهانه", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (ShiftSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد ", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (ShiftovertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (Shift == Shift.ByMounth && InsurancePremium <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("مبلغ بیمه", "صفر"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (Shift == Shift.ByMounth && DayInMonth <= 0)
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

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.workerManager.Create(
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
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                await db.SaveChangesAsync();
            }

            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("CustomerList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
