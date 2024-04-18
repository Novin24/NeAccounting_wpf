using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;
public partial class FromTheSellViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService) : ObservableObject
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;
    private readonly IContentDialogService _contentDialogService = contentDialogService;


    private int rowId = 1;

    /// <summary>
    /// لیست اجناس  فاکتور  فروش
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _sellGoods = [];

    /// <summary>
    /// لیست اجناس  برگشتی
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _list = [];

    /// <summary>
    /// لیست کلیه اجناس
    /// </summary>
    [ObservableProperty]
    private List<MatListDto> _matList;

    /// <summary>
    /// شناسه مشتری
    /// </summary>
    [ObservableProperty]
    private long _CusId;

    /// <summary>
    /// نام مشتری
    /// </summary>
    [ObservableProperty]
    private long _CusName;

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
    /// شماره فاکتور
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

    /// <summary>
    /// افزودن ردیف
    /// </summary>
    /// <returns></returns>
    internal async Task<bool> OnAdd()
    {
        #region validation
        if (MaterialId < 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (MatPrice == null || MatPrice == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        long maxPrice = SellGoods.Where(c => c.MaterialId == MaterialId).Max(t => t.Price);
        if (MatPrice > maxPrice)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = $"مبلغ وارد شده بیشتر از قیمت فروش ({maxPrice:N0}) می‌باشد \n آیا ادامه میدهید!!!", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result != ContentDialogResult.Primary) return false;
        }

        if (AmountOf == null || AmountOf <= 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        double totalAmountOf = SellGoods.Where(c => c.MaterialId == MaterialId).Sum(t => t.AmountOf);
        if (AmountOf > totalAmountOf)
        {
            var matName = SellGoods.First(c => c.MaterialId == MaterialId);
            _snackbarService.Show("خطا", $"مقدار وارد شده بیشتر از مقدار فروش ({totalAmountOf} _ {matName})", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        //if (AmountOf > MatList.First(t => t.Id == MaterialId).Entity)
        //{
        //    _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
        //}

        var mat = MatList.First(t => t.Id == MaterialId);
        if (mat.IsService)
        {
            _snackbarService.Show("خطا", "اجناس برگشتی شامل سرویس نمیتواند باشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
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
            MaterialId = MaterialId,
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
        Credit = s.Credit;
        Debt = s.Debt;
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
    [RelayCommand]
    private async Task OnSumbit()
    {
        #region validation

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (List == null || List.Count == 0)
        {
            _snackbarService.Show("خطا", "وارد کردن حداقل یک ردیف الزامیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (string.IsNullOrEmpty(InvDescription))
        {
            InvDescription = "فاکتور اجناس برگشت از فروش";
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
                return;
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

        Type? pageType = NameToPageTypeConverter.Convert("Bill");
        if (pageType == null)
        {
            return;
        }
        _navigationService.Navigate(pageType);
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
    [RelayCommand]
    private void OnAddClick(string parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter))
        {
            return;
        }

        Type? pageType = NameToPageTypeConverter.Convert(parameter);

        if (pageType == null)
        {
            return;
        }

        _ = _navigationService.Navigate(pageType);
    }
}
