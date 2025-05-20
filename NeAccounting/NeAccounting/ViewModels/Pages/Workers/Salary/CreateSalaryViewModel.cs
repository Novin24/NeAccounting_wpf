using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreateSalaryViewModel : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService;
    private readonly INavigationService _navigationService;
    private bool _isreadonly = true;

    /// <summary>
    /// شناسه کارگر
    /// </summary>
    [ObservableProperty]
    private Guid? _workerId = null;

    /// <summary>
    /// شماره پرسنلی
    /// </summary>
    [ObservableProperty]
    private int? _personelId;

    /// <summary>
    /// ماه
    /// </summary>
    [ObservableProperty]
    private byte? _submitMonth;

    /// <summary>
    /// سال
    /// </summary>
    [ObservableProperty]
    private int? _submitYear;

    /// <summary>
    /// کارکرد عادی
    /// </summary>
    [ObservableProperty]
    private long _amountOf = 0;

    /// <summary>
    /// مساعده مالی
    /// </summary>
    [ObservableProperty]
    private long _financialAid = 0;

    /// <summary>
    /// اضافه کاری
    /// </summary>
    [ObservableProperty]
    private long _overTime = 0;

    /// <summary>
    /// مالیات
    /// </summary>
    [ObservableProperty]
    private long _tax = 0;

    /// <summary>
    /// حق اولاد
    /// </summary>
    [ObservableProperty]
    private long _childAllowance = 0;

    /// <summary>
    /// بیمه
    /// </summary>
    [ObservableProperty]
    private long _insurance = 0;

    /// <summary>
    /// حق خوار و بار و مسکن
    /// </summary>
    [ObservableProperty]
    private long _rightHousingAndFood = 0;

    /// <summary>
    /// قسط وام
    /// </summary>
    [ObservableProperty]
    private long _loanInstallment = 0;

    /// <summary>
    /// سایر اضافات
    /// </summary>
    [ObservableProperty]
    private long _otherAdditions = 0;

    /// <summary>
    /// سایر کسورات
    /// </summary>
    [ObservableProperty]
    private long _otherDeductions = 0;

    /// <summary>
    /// باقی مانده
    /// </summary>
    [ObservableProperty]
    private long _leftOver = 0;

    /// <summary>
    /// توضیحات
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    /// شیفت کاری
    /// </summary>
    [ObservableProperty]
    private Shift _shiftStatus;

    /// <summary>
    /// سرچ پرسنل
    /// </summary>
    [ObservableProperty]
    private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

    /// <summary>
    /// غیرفعال بودن سرچ
    /// </summary>
    [ObservableProperty]
    private bool _loding = true;

    /// <summary>
    /// متن نمایشی سرچ
    /// </summary>
    [ObservableProperty]
    private string _placeholderSearch = "در حال بارگذاری ...";

    public CreateSalaryViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
        PersianCalendar pc = new();
        SubmitMonth = (byte)pc.GetMonth(DateTime.Now);
        SubmitYear = pc.GetYear(DateTime.Now);
        _isreadonly = NeAccountingConstants.ReadOnlyMode;
    }

    [RelayCommand]
    private async Task OnCreate()
    {
        #region validation
        if (_isreadonly)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        if (WorkerId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام پرسنلی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (AmountOf <= 0)
        {
            _snackbarService.Show("خطا", "پرسنل یا تاریخ وارد شده نامعتبر!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (FinancialAid < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مساعده"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (OverTime < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اضافه کاری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (SubmitMonth == null || SubmitYear == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (Tax < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مالیات"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (RightHousingAndFood < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("حق خوار و بار و مسکن"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (LoanInstallment < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("وام"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (OtherAdditions < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سایر اضافات"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (OtherDeductions < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سایر کسورات"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        #endregion

        using (UnitOfWork db = new())
        {
            var (error, isSuccess) = await db.WorkerManager.AddSalary(
                   WorkerId.Value,
                   SubmitMonth.Value,
                   SubmitYear.Value,
                   AmountOf,
                   FinancialAid,
                   OverTime,
                   Tax,
                   ChildAllowance,
                   RightHousingAndFood,
                   Insurance,
                   LoanInstallment,
                   OtherAdditions,
                   OtherDeductions,
                   LeftOver,
                   Description);

            if (!isSuccess)
            {
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
        }

        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        Type? pageType = NameToPageTypeConverter.Convert("SalaryList");

        if (pageType == null)
        {
            return;
        }
        _ = _navigationService.Navigate(pageType);
    }

    public async void OnNavigatedTo()
    {
        using UnitOfWork db = new();
        AuSuBox = await db.WorkerManager.GetDisplayWorkers();
        Loding = false;
        PlaceholderSearch = "جستجو ...";
    }

    public void OnNavigatedFrom()
    {
    }

    public async Task<bool> OnSelect()
    {
        if (WorkerId == null || SubmitMonth == null || SubmitYear == null)
        {
            return false;
        }
        using UnitOfWork db = new();
        var worker = await db.WorkerManager.GetWorker(WorkerId.Value);
        var details = await db.WorkerManager.GetSalaryDetailByWorkerId(WorkerId.Value, SubmitMonth.Value, SubmitYear.Value);

        if (!details.Success)
        {
            _snackbarService.Show("کاربر گرامی", details.Error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        ShiftStatus = worker.Shift;
        Insurance = details.Insurance;
        AmountOf = details.AmountOf;
        OverTime = details.OverTime;
        PersonelId = details.PersonelId;
        FinancialAid = details.FinancialAid;

        return true;
    }
}
