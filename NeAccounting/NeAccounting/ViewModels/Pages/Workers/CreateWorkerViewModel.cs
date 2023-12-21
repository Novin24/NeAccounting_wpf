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
        private int? _personalId;

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
        private byte _status = 0;

        [ObservableProperty]
        private long? _salary;

        [ObservableProperty]
        private long? overtimeSalary;

        [ObservableProperty]
        private long? insurancePremium;

        [ObservableProperty]
        private byte? dayInMonth;


        [RelayCommand]
        private async Task OnCreate()
        {

            if (string.IsNullOrEmpty(FullName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کارگر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("عنوان شغل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موبایل"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("آدرس"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (PersonalId == null || PersonalId <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("شماره پرسنلی", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (Salary == null || Salary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (OvertimeSalary == null || OvertimeSalary <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("دستمزد اضافه کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (Shift == Shift.ByMounth && (InsurancePremium == null || InsurancePremium <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("مبلغ بیمه", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (Shift == Shift.ByMounth && (DayInMonth == null || DayInMonth <= 0))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMore("تعداد روز کاری", "صفر"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            if (string.IsNullOrEmpty(AccountNumber))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("شماره حساب"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
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
                       PersonalId.Value,
                       AccountNumber,
                       Description,
                       JobTitle,
                       StartDate,
                       Shift,
                       Salary.Value,
                       OvertimeSalary.Value,
                       InsurancePremium.Value,
                       DayInMonth.Value);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
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
