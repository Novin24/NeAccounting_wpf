using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreateSalaryViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    private int _workerId = -1;

    [ObservableProperty]
    private int? _personelId;

    [ObservableProperty]
    private DateTime _submitDate = DateTime.Now;

    [ObservableProperty]
    private uint _amountOf = 0;

    [ObservableProperty]
    private uint _financialAid = 0;

    [ObservableProperty]
    private uint _overTime = 0;

    [ObservableProperty]
    private uint _tax = 0;

    [ObservableProperty]
    private uint _childAllowance = 0;

    [ObservableProperty]
    private uint _insurance = 0;

    [ObservableProperty]
    private uint _rightHousingAndFood = 0;

    [ObservableProperty]
    private uint _loanInstallment = 0;

    [ObservableProperty]
    private uint _otherAdditions = 0;

    [ObservableProperty]
    private uint _otherDeductions = 0;

    [ObservableProperty]
    private uint _leftOver = 0;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private Shift _shiftStatus;

    [ObservableProperty]
    private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;


    [RelayCommand]
    private async Task OnCreate()
    {
        if (WorkerId == -1)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام پرسنلی"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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

        using (UnitOfWork db = new())
        {
            var (error, isSuccess) = await db.workerManager.AddOrUpdateSalary(
                   WorkerId,
                   SubmitDate,
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
            await db.SaveChangesAsync();
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
        AuSuBox = await db.workerManager.GetWorkers();
    }

    public void OnNavigatedFrom()
    {
    }

    public async Task<bool> OnSelect()
    {
        if (WorkerId == -1)
        {
            return false;
        }
        using UnitOfWork db = new();
        var Worker = await db.workerManager.GetWorker(WorkerId);
        var details = await db.workerManager.GetSalaryDetailByWorkerId(WorkerId, SubmitDate);

        if (!details.Success)
        {
            _snackbarService.Show("کاربر گرامی", details.Error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        ShiftStatus = Worker.Shift;
        Insurance = details.Insurance;
        AmountOf = details.AmountOf;
        OverTime = details.OverTime;
        PersonelId = details.PersonelId;
        FinancialAid = details.FinancialAid;

        return true;
    }
}
