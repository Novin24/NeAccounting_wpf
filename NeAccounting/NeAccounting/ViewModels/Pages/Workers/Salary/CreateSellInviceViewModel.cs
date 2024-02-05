using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreateSellInviceViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;


    [ObservableProperty]
    private ICollection<RemittanceListViewModel> _list = new List<RemittanceListViewModel>();


    [ObservableProperty]
    private List<SuggestBoxViewModel<Guid>> _cuslist;

    [ObservableProperty]
    private int _materialId = -1;

    [ObservableProperty]
    private double _amountOf = -1;

    [ObservableProperty]
    private uint _totalPrice = 0;

    [ObservableProperty]
    private string? _description;

    [RelayCommand]
    private async Task OnCreate()
    {

       
    }

    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser(false,true);
    }

    public void OnNavigatedFrom()
    {
    }

    internal bool OnAdd()
    {

        if (MaterialId < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (TotalPrice == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        List.Add(new RemittanceListViewModel()
        {
            AmountOf = AmountOf,
            TotalPrice = TotalPrice,
            Description = Description,
            MaterialId = MaterialId,
        });
        return true;
    }
}
