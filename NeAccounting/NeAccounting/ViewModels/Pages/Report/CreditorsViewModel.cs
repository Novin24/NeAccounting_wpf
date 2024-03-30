using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class CreditorsViewModel : ObservableObject, INavigationAware
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
    private IEnumerable<CreditorsOrDebtorsReport> _creList;
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
        CreList = await db.DocumentManager.GetcreditorsReport(Min, Max);
    }

    [RelayCommand]
    private async Task OnSearch()
    {
        using UnitOfWork db = new();
        CreList = await db.DocumentManager.GetcreditorsReport(Min, Max);
    }
    #endregion
}




