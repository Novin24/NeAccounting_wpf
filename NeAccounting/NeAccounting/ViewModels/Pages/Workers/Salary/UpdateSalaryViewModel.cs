using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class UpdateSalaryViewModel : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService;
    private readonly INavigationService _navigationService;
    private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


    [ObservableProperty]
    private Guid _workerId ;

    [ObservableProperty]
    private int _salaryId = -1;

    [ObservableProperty]
    private int? _personnelId;

    [ObservableProperty]
    private string _personnelName;

    [ObservableProperty]
    private byte? _submitMonth;

    [ObservableProperty]
    private int? _submitYear;

    [ObservableProperty]
    private long _amountOf;

    [ObservableProperty]
    private long _financialAid;

    [ObservableProperty]
    private long _overTime;

    [ObservableProperty]
    private long _tax;

    [ObservableProperty]
    private long _childAllowance;

    [ObservableProperty]
    private long _insurance;

    [ObservableProperty]
    private long _rightHousingAndFood;

    [ObservableProperty]
    private long _loanInstallment;

    [ObservableProperty]
    private long _otherAdditions;

    [ObservableProperty]
    private long _otherDeductions;

    [ObservableProperty]
    private long _leftOver;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private Shift _shiftStatus;

    public UpdateSalaryViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
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
        if (AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تعداد روز / شیفت کاری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (FinancialAid < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مساعده"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (SubmitMonth == null || SubmitYear == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پرداخت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (OverTime < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اضافه کاری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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
            var (error, isSuccess) = await db.WorkerManager.UpdateSalary(
                   WorkerId,
                   SalaryId,
                   SubmitYear.Value,
                   SubmitMonth.Value,
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

    public void OnNavigatedTo()
    {

    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    public async Task<bool> OnSelect()
    {
        if ( SubmitMonth == null || SubmitYear == null)
        {
            return false;
        }
        using UnitOfWork db = new();
        var Worker = await db.WorkerManager.GetWorker(WorkerId);
        var details = await db.WorkerManager.GetSalaryDetailByWorkerId(WorkerId, SubmitMonth.Value, SubmitYear.Value, SalaryId);

        if (!details.Success)
        {
            _snackbarService.Show("کاربر گرامی", details.Error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        ShiftStatus = Worker.Shift;
        Insurance = details.Insurance;
        AmountOf = details.AmountOf;
        OverTime = details.OverTime;
        PersonnelId = details.PersonelId;
        FinancialAid = details.FinancialAid;
        RightHousingAndFood = details.RightHousingAndFood;
        ChildAllowance = details.ChildAllowance;
        OtherAdditions = details.OtherAdditions;
        Insurance = details.Insurance;
        Tax = details.Tax;
        LoanInstallment = details.LoanInstallment;
        OtherDeductions = details.OtherDeductions;
        Description = details.Description;

        return true;
    }
}
