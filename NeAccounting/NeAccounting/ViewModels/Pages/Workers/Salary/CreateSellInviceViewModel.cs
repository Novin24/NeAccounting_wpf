using Domain.NovinEntity.Documents;
using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class CreateSellInviceViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;

    private int rowId = 1;

    [ObservableProperty]
    private ICollection<RemittanceListViewModel> _list = new List<RemittanceListViewModel>();

    [ObservableProperty]
    private List<SuggestBoxViewModel<Guid, long>> _cuslist;

    [ObservableProperty]
    private List<MatListDto> _matList;

    [ObservableProperty]
    private Guid? _CusId;

    [ObservableProperty]
    private string _lastInvoice;

    [ObservableProperty]
    private int _materialId = -1;

    [ObservableProperty]
    private double? _amountOf;

    [ObservableProperty]
    private long? _matPrice;

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
        Cuslist = await db.CustomerManager.GetDisplayUser(null, true);
        LastInvoice = await db.DocumentManager.GetLastDocumntNumber(DocumntType.SellInv);
        MatList = await db.MaterialManager.GetMaterails();
    }

    public void OnNavigatedFrom()
    {
    }

    internal bool OnAdd()
    {

        if (CusId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (MaterialId < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (AmountOf == null || AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (AmountOf > MatList.First(t => t.Id == MaterialId).Entity)
        {
            _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
        }

        if (MatPrice == null || MatPrice == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        var mat = MatList.First(t => t.Id == MaterialId);
        List.Add(new RemittanceListViewModel()
        {
            AmountOf = AmountOf.Value,
            UnitName = mat.UnitName,
            MatName = mat.MaterialName,
            Price = MatPrice.Value,
            RowId = rowId,
            TotalPrice = (uint)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId,
        });
        rowId++;
        return true;
    }
}
