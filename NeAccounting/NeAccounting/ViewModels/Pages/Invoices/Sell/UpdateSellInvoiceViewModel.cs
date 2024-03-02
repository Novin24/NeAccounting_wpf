using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class UpdateSellInvoiceViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;
    private readonly ISnackbarService _snackbarService;
    private readonly INavigationService _navigationService;

    public UpdateSellInvoiceViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
    }

    public int RowId = 1;

    public Guid InvoiceId { get; set; }

    /// <summary>
    /// لیست اجناس  فاکتور
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _list = [];

    /// <summary>
    /// لیست کلیه اجناس
    /// </summary>
    [ObservableProperty]
    private List<MatListDto> _matList;

    /// <summary>
    /// نام مشتری
    /// </summary>
    [ObservableProperty]
    private string _cusName;

    /// <summary>
    /// شماره مشتری
    /// </summary>
    [ObservableProperty]
    private long _cusNumber;

    [ObservableProperty]
    private DateTime? _submitDate = DateTime.Now;

    /// <summary>
    /// مقدار پورسانت
    /// </summary>
    [ObservableProperty]
    private double? _commission;

    /// <summary>
    /// وضعیت مشتری
    /// </summary>
    [ObservableProperty]
    private string _status = "تسویه";

    /// <summary>
    /// بدهکاری مشتری
    /// </summary>
    [ObservableProperty]
    private string _debt = "0";

    /// <summary>
    /// طلبکاری مشتری
    /// </summary>
    [ObservableProperty]
    private string _credit = "0";

    /// <summary>
    /// مبلغ کل فاکتور
    /// </summary>
    [ObservableProperty]
    private string _totalPrice = "0";

    /// <summary>
    /// مبلغ کل پورسانت
    /// </summary>
    [ObservableProperty]
    private string _totalcommission = "0";

    /// <summary>
    /// مبلغ باقی مانده
    /// </summary>
    [ObservableProperty]
    private string _remainPrice = "0";

    /// <summary>
    /// شماره اخرین فاکتور
    /// </summary>
    [ObservableProperty]
    private string _lastInvoice;

    /// <summary>
    /// شناسه جنس انتخاب شده در سلکت باکس
    /// </summary>
    [ObservableProperty]
    private int _materialId = -1;

    /// <summary>
    /// مقدار انتخاب شده
    /// </summary>
    [ObservableProperty]
    private double? _amountOf;

    /// <summary>
    /// مبلغ انتخابی 
    /// </summary>
    [ObservableProperty]
    private long? _matPrice;

    /// <summary>
    /// توضیحات ردیف
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    /// توضیحات فاکتور
    /// </summary>
    [ObservableProperty]
    private string? _invDescription;

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom()
    {
    }
    private void InitializeViewModel()
    {
        _isInitialized = true;
    }


    /// <summary>
    /// افزودن ردیف
    /// </summary>
    /// <returns></returns>
    internal bool OnAdd()
    {
        #region validation


        //if (SubmitDate == null)
        //{
        //    _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return false;
        //}

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
        #endregion

        var mat = MatList.First(t => t.Id == MaterialId);
        List.Add(new RemittanceListViewModel()
        {
            AmountOf = AmountOf.Value,
            UnitName = mat.UnitName,
            MatName = mat.MaterialName,
            Price = MatPrice.Value,
            RowId = RowId,
            TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId,
        });
        SetCommisionValue();
        RefreshRow(ref RowId);
        return true;
    }


    /// <summary>
    /// ویرایش ردیف
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns></returns>
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

    /// <summary>
    /// حذف ردیف
    /// </summary>
    /// <param name="rowId"></param>s
    internal void OnRemove(int rowId)
    {
        var itm = List.FirstOrDefault(t => t.RowId == rowId);
        if (itm != null)
        {
            List.Remove(itm);
            RefreshRow(ref rowId);
        }
    }

    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    internal async Task<bool> OnSumbit()
    {
        #region validation
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
            var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false, item.Price);
            if (!isSuccess)
            {
                _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return false;
            }
        }
        #endregion

        #region CreateSellDoc
        var totalInvoicePrice = List.Sum(t => t.TotalPrice);

        //var (e, s) = await db.DocumentManager.CreateSellDocument(CusId.Value, totalInvoicePrice, Commission, InvDescription, SubmitDate.Value, List);
        //if (!s)
        //{
        //    _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //    return false;
        //}
        await db.SaveChangesAsync();
        #endregion

        #region reload
        _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور با موفقیت انجام شد", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        await Reload();
        return true;
        #endregion
    }

    /// <summary>
    /// به روز رسانی شماره ردیف ها
    /// </summary>
    /// <param name="rowId"></param>
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

    /// <summary>
    /// بارگیری مجدد صفحه و خالی کردن تمام اینپوت ها
    /// </summary>
    /// <returns></returns>
    private async Task Reload()
    {
        using UnitOfWork db = new();
        LastInvoice = await db.DocumentManager.GetLastDocumntNumber(DocumntType.SellInv);
        List = [];
        Commission = null;
        MaterialId = -1;
        InvDescription = null;
        Description = null;
        SubmitDate = DateTime.Now;
        MatPrice = null;
        Totalcommission = "0";
        TotalPrice = "0";
        RemainPrice = "0";
        Status = "تسویه";
        Debt = "0";
        Credit = "0";
    }

    /// <summary>
    /// به روز رسانی مبلغ پورسانت
    /// </summary>
    private void SetCommisionValue()
    {
        long total = List.Sum(t => t.TotalPrice);
        TotalPrice = total.ToString("N0");
        if (Commission != null && Commission != 0)
        {
            var com = (long)(total * (Commission / 100));
            Totalcommission = com.ToString("N0");
            total -= com;
        }
        else
        {
            Totalcommission = "0";
        }
        RemainPrice = total.ToString("N0");
    }
}
