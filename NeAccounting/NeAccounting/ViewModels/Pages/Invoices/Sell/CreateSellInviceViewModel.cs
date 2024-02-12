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
    private List<RemittanceListViewModel> _list = [];

    [ObservableProperty]
    private List<SuggestBoxViewModel<Guid, long>> _cuslist;

    [ObservableProperty]
    private List<MatListDto> _matList;

    [ObservableProperty]
    private Guid? _CusId;

    [ObservableProperty]
    private DateTime? _submitDate;

    [ObservableProperty]
    private int? _commission;

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

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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
            Price = (uint)MatPrice.Value,
            RowId = rowId,
            TotalPrice = (uint)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId,
        });
        RefreshRow(ref rowId);
        return true;
    }

    internal (bool, RemittanceListViewModel) OnUpdate(int rowId)
    {
        var itm = List.FirstOrDefault(t => t.RowId == rowId);
        if (itm == null)
            return new(false, new RemittanceListViewModel());
        MaterialId = itm.MaterialId;
        AmountOf = itm.AmountOf;
        MatPrice = itm.Price;
        Description = itm.Description;
        List.Remove(itm);
        RefreshRow(ref rowId);
        return new(true, itm);
    }

    internal void OnRemove(int rowId)
    {
        var itm = List.FirstOrDefault(t => t.RowId == rowId);
        if (itm != null)
        {
            List.Remove(itm);
            RefreshRow(ref rowId);
        }
    }

    internal async Task<bool> OnSumbit()
    {
        #region validation
        if (CusId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (List == null || List.Count == 0)
        {
            _snackbarService.Show("خطا", "وارد کردن حداقل یک ردیف الزامیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        #endregion

        #region UpdateMaterial
        using UnitOfWork db = new();
        foreach (var item in List)
        {
            var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false);
            if (!isSuccess)
            {
                _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
        }
        #endregion

        #region CreateSellDoc
        var totalInvoicePrice = (uint)List.Sum(t => t.TotalPrice);

        var (e, s, serial) = await db.DocumentManager.CreateSellDocument(CusId.Value, totalInvoicePrice, Description, SubmitDate.Value, false, List);
        if (!s)
        {
            _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        #endregion

        #region create_Commission_Doc
        if (Commission != null && Commission != 0)
        {
            var (er, su, sr) = await db.DocumentManager.CreateDocument(CusId.Value, (uint)(totalInvoicePrice * (Commission / 100)),
                DocumntType.Rec, $"{serial} پورسانت فاکتور", SubmitDate.Value, true);

            if (!su)
            {
                _snackbarService.Show("خطا", er, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
        }
        await db.SaveChangesAsync();
        #endregion

        #region reload
        _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور به شماره {serial}", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        await Reload();
        return true;
        #endregion
    }


    private void RefreshRow(ref int rowId)
    {
        int row = 1;
        foreach (var item in List.OrderBy(t => t.RowId))
        {
            item.RowId = row;
            row++;
        }
        rowId = row;
    }

    private async Task Reload()
    {
        using UnitOfWork db = new();
        LastInvoice = await db.DocumentManager.GetLastDocumntNumber(DocumntType.SellInv);
        List = [];
        CusId = null;
        Commission = null;
        MaterialId = -1;
        Description = null;
        SubmitDate = null;
        MatPrice = null;
    }
}
