using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreateSalaryViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    private int _workerId = -1;

    [ObservableProperty]
    private DateTime _submitDate = DateTime.Now;

    [ObservableProperty]
    private int _amountOf;

    [ObservableProperty]
    private uint _financialAid;

    [ObservableProperty]
    private uint _overTime;

    [ObservableProperty]
    private uint _tax;

    [ObservableProperty]
    private uint _childAllowance;

    [ObservableProperty]
    private uint _insurance;

    [ObservableProperty]
    private uint _rightHousingAndFood;

    [ObservableProperty]
    private uint _loanInstallment;

    [ObservableProperty]
    private uint _otherAdditions;

    [ObservableProperty]
    private uint _otherDeductions;

    [ObservableProperty]
    private long _leftOver;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private IEnumerable<SuggestBoxViewModel<int>> _auSuBox;

    [RelayCommand]
    private async Task OnCreate()
    {
        if (WorkerId == -1)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام پرسنلی"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تعداد روز / شیفت کاری"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (FinancialAid < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مساعده"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (OverTime < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("اضافه کاری"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (Tax < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مالیات"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (RightHousingAndFood < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("حق خوار و بار و مسکن"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (LoanInstallment < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("وام"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (OtherAdditions < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سایر اضافات"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        if (OtherDeductions < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سایر کسورات"), ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
            return;
        }

        using (UnitOfWork db = new())
        {
            var (error, isSuccess) = await db.workerManager.AddSalary(
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

    public async void OnNavigatedTo()
    {
        using UnitOfWork db = new();
        AuSuBox = await db.workerManager.GetWorkers();
    }

    public void OnNavigatedFrom()
    {
    }
}
