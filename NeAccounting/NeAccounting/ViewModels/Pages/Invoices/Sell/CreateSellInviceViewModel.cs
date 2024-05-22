using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Extension;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Models;
using NeAccounting.Resources;
using NeAccounting.Windows;
using NeApplication.Services;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class CreateSellInvoiceViewModel(ISnackbarService snackbarService, WindowsProviderService serviceProvider, IPrintServices printServices) : ObservableObject, INavigationAware
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly IPrintServices _printServices = printServices;
    private readonly WindowsProviderService _windowsProviderService = serviceProvider;
    private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;


    #region Property
    /// <summary>
    /// مبلغ باقی مانده
    /// </summary>
    private long _longRemainPrice = 0;

    private int rowId = 1;

    /// <summary>
    /// لیست اجناس  فاکتور
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _list = [];

    /// <summary>
    /// لیست مشتری ها
    /// </summary>
    [ObservableProperty]
    private List<SuggestBoxViewModel<Guid, long>> _cuslist;

    /// <summary>
    /// لیست کلیه اجناس
    /// </summary>
    [ObservableProperty]
    private List<MatListDto> _matList;

    /// <summary>
    /// شناسه مشتری
    /// </summary>
    [ObservableProperty]
    private Guid? _CusId;

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
    private Guid? _materialId = null;

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
    /// پرینت پس از تایید
    /// </summary>
    [ObservableProperty]
    private bool _print = false;

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
    #endregion

    #region Method

    public async void OnNavigatedTo()
    {
        await InitializeViewModel();
    }

    private async Task InitializeViewModel()
    {
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser(false, null, true);
        LastInvoice = await db.DocumentManager.GetLastDocumntNumber(DocumntType.SellInv);
        MatList = await db.MaterialManager.GetMaterails();
    }

    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// افزودن ردیف
    /// </summary>
    /// <returns></returns>
    internal bool OnAdd()
    {
        #region validation

        if (_isreadonly)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        if (CusId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }


        if (MaterialId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (AmountOf == null || AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }


        if (MatPrice == null || MatPrice == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        var mat = MatList.First(t => t.Id == MaterialId);
        if (!mat.IsService && AmountOf > MatList.First(t => t.Id == MaterialId).Entity)
        {
            _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
        }
        #endregion

        List.Add(new RemittanceListViewModel()
        {
            AmountOf = AmountOf.Value,
            UnitName = mat.UnitName,
            IsService = mat.IsService,
            MatName = mat.MaterialName,
            Price = MatPrice.Value,
            RowId = rowId,
            TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId.Value,
        });
        SetCommisionValue();
        RefreshRow(ref rowId);
        return true;
    }

    /// <summary>
    /// انتخاب مشتری
    /// </summary>
    /// <param name="custId"></param>
    /// <returns></returns>
    internal async Task OnSelectCus(Guid custId)
    {
        using UnitOfWork db = new();
        var s = await db.DocumentManager.GetStatus(custId);
        Status = s.Status;
        Debt = s.Amount;
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
    /// <param name="rowId"></param>
    internal void OnRemove(int rowId)
    {
        var itm = List.FirstOrDefault(t => t.RowId == rowId);
        if (itm != null)
        {
            List.Remove(itm);
            RefreshRow(ref rowId);
        }
        SetCommisionValue();
    }

    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
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

        if (string.IsNullOrEmpty(InvDescription))
        {
            InvDescription = "فاکتور فروش";
        }
        #endregion

        #region UpdateMaterial
        using UnitOfWork db = new();
        foreach (var item in List)
        {
            if (item.IsService) continue;

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

        var (e, s) = await db.DocumentManager.CreateSellDocument(CusId.Value, totalInvoicePrice, Commission, InvDescription, SubmitDate.Value, List);
        if (!s)
        {
            _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        await db.SaveChangesAsync();
        #endregion

        #region reload
        _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور با موفقیت انجام شد", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
        if (Print) PrintOneInvoice();

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
        MatList = await db.MaterialManager.GetMaterails();
        List = [];
        CusId = null;
        Commission = null;
        MaterialId = null;
        InvDescription = null;
        Print = false;
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
        _longRemainPrice = total;
        RemainPrice = total.ToString("N0");
    }

    [RelayCommand]
    private async Task OnAddClick()
    {
        _windowsProviderService.ShowDialog<CreateCustomerWindow>();
        using UnitOfWork db = new();
        Cuslist = await db.CustomerManager.GetDisplayUser(false, true);
    }

    private void PrintOneInvoice()
    {
        var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
        if (printInfo == null)
        {
            _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        var cus = Cuslist.First(t => t.Id == CusId);
        PersianCalendar pc = new();
        Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"({cus.UniqNumber}) _ {cus.DisplayName}"},
                {"SubmitTime",$"{SubmitDate.ToShamsiDate(pc)}"},
                {"PrintTime",DateTime.Now.ToShamsiDate(pc) },
                {"Total_InvoicePrice",TotalPrice},
                {"Commission",Totalcommission},
                {"LeftOverPrice",RemainPrice},
                {"TotalSLeftOver",_longRemainPrice.ToString().NumberToPersianString()},
                {"Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{printInfo.Tabligh}"}
            };

        _printServices.PrintInvoice(@"Required\Reports\ReportOneInvoice.mrt", "DetailListDtos", List, dic);
    }
    #endregion

}
