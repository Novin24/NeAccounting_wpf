using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class DebtorsViewModel : ObservableObject, INavigationAware
{

    #region Properties

    /// <summary>
    /// حداقل
    /// </summary>
    [ObservableProperty]
    private long _min = 0;

    /// <summary>
    /// حداکثر
    /// </summary>
    [ObservableProperty]
    private long _max = 0;

    /// <summary>
    /// لیست بدهکاران
    /// </summary>
    [ObservableProperty]
    private IEnumerable<CreditorsOrDebtorsReport> _debList;
    #endregion

    #region Methods
    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    public void OnNavigatedFrom()
    {

    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        DebList = await db.DocumentManager.GetDebtorsReport(Min, Max);
    }

    [RelayCommand]
    private async Task OnSearch()
    {
        using UnitOfWork db = new();
        DebList = await db.DocumentManager.GetDebtorsReport(Min, Max);
    }
    #endregion
}




