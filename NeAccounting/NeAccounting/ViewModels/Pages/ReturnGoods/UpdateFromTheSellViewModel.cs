using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class UpdateFromTheSellViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService) : ObservableObject
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;
    private readonly IContentDialogService _contentDialogService = contentDialogService;


    private int rowId = 1;

    /// <summary>
    /// لیست اجناس  فاکتور  فروش
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _sellGoods;

    /// <summary>
    /// لیست ثابت اجناس برگشتی فاکتور
    /// </summary>
    public List<RemittanceListViewModel> StaticList = [];

    /// <summary>
    /// لیست اجناس  برگشتی
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<RemittanceListViewModel> _list = [];

    /// <summary>
    /// لیست کلیه اجناس
    /// </summary>
    [ObservableProperty]
    private List<MatListDto> _matList;

    /// <summary>
    /// شماره مشتری
    /// </summary>
    [ObservableProperty]
    private long _CusNum;


    /// <summary>
    /// شناسه فاکتور خرید
    /// </summary>
    [ObservableProperty]
    private Guid _parentDocId;

    /// <summary>
    /// شناسه فاکتور برگشتی
    /// </summary>
    [ObservableProperty]
    private Guid _returndocId;

    /// <summary>
    /// ردیف انتخاب شده
    /// </summary>
    [ObservableProperty]
    private Guid? _remId;

    /// <summary>
    /// شناسه مشتری
    /// </summary>
    [ObservableProperty]
    private Guid _cusId;

    /// <summary>
    /// نام مشتری
    /// </summary>
    [ObservableProperty]
    private string _CusName;

    /// <summary>
    /// تاریخ بازگشت اجناس
    /// </summary>
    [ObservableProperty]
    private DateTime? _submitDate = DateTime.Now;

    /// <summary>
    /// مبلغ کل فاکتور
    /// </summary>
    [ObservableProperty]
    private string _totalPrice = "0";


    /// <summary>
    /// شماره فاکتور فروش
    /// </summary>
    [ObservableProperty]
    private string _parentInvoiceSerial;

    /// <summary>
    /// شماره فاکتور برگشتی
    /// </summary>
    [ObservableProperty]
    private string _returnInvoicSerial;

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
        double listAmountOf = List.Where(c => c.MaterialId == MaterialId).Sum(t => t.AmountOf);
        if ((listAmountOf + AmountOf) > totalAmountOf)
        {
            var unitName = MatList.First(c => c.Id == MaterialId).UnitName;
            _snackbarService.Show("خطا", $"مقدار وارد شده بیشتر از مقدار فروش ({totalAmountOf} _ {unitName})", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        //if (AmountOf > MatList.First(t => t.Id == MaterialId).Entity)
        //{
        //    _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
        //}

        var mat = MatList.First(t => t.Id == MaterialId);
        if (mat.IsService)
        {
            _snackbarService.Show("خطا", "اجناس برگشتی شامل خدمات نمیتواند باشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        #endregion

        List.Add(new RemittanceListViewModel()
        {
            AmountOf = AmountOf.Value,
            UnitName = mat.UnitName,
            IsService = mat.IsService,
            MatName = mat.MaterialName,
            RremId = RemId ?? Guid.Empty,
            Price = MatPrice.Value,
            RowId = rowId,
            TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId,
        });
        long total = List.Sum(t => t.TotalPrice);
        SetTotal();
        RefreshRow(ref rowId);
        return true;
    }

    /// <summary>
    /// ویرایش ردیف
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns></returns>
    internal (bool, RemittanceListViewModel) OnUpdate(int rowId)
    {
        if (RemId != null) return (false, new RemittanceListViewModel());

        var itm = List.FirstOrDefault(t => t.RowId == rowId);

        if (itm == null) return (false, new RemittanceListViewModel());

        MaterialId = itm.MaterialId;
        RemId = itm.RremId;
        AmountOf = itm.AmountOf;
        MatPrice = itm.Price;
        Description = itm.Description;
        List.Remove(itm);
        RefreshRow(ref rowId);
        SetTotal();
        return new(true, itm);
    }

    /// <summary>
    /// حذف ردیف
    /// </summary>
    /// <param name="rowId"></param>
    [RelayCommand]
    internal void OnRemove(int rowId)
    {
        var itm = List.FirstOrDefault(t => t.RowId == rowId);
        if (itm == null)
            return;
        if (itm.RremId != Guid.Empty)
        {
            itm.IsDeleted = !itm.IsDeleted;
        }
        else
        {
            List.Remove(itm);
            RefreshRow(ref rowId);
        }
        SetTotal();
    }

    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task OnSubmit()
    {
        #region validation


        if (RemId != null)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (SubmitDate == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        if (List == null || !List.Any(t => !t.IsDeleted))
        {
            _snackbarService.Show("خطا", "وارد کردن حداقل یک ردیف الزامیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }

        #endregion

        #region UpdateMaterial
        using UnitOfWork db = new();
        var li = new List<RemittanceListViewModel>(List.Where(t => !t.IsDeleted || t.RremId != Guid.Empty));
        foreach (var item in li)
        {
            if (item.RremId == Guid.Empty)
            {
                var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, true, item.Price);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                continue;
            }
            var oldItm = StaticList.First(t => t.RremId.Equals(item.RremId));

            if (oldItm.AmountOf == item.AmountOf)
                continue;

            if (item.AmountOf < oldItm.AmountOf)
            {
                var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, oldItm.AmountOf - item.AmountOf, false, item.Price);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                continue;
            }
            else
            {
                var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf - oldItm.AmountOf, true, item.Price);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                continue;
            }

        }
        #endregion

        #region UpdateDoc
        var totalInvoicePrice = List.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
        var (e, s) = await db.DocumentManager.UpdateReturnFromTheSellInvoice(ParentDocId, ReturndocId, totalInvoicePrice, InvDescription, SubmitDate.Value, [.. List]);
        if (!s)
        {
            _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return;
        }
        await db.SaveChangesAsync();
        #endregion

        #region RedirectToList
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
    /// به روز رسانی مبلغ کل
    /// </summary>
    private void SetTotal()
    {
        long total = List.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
        TotalPrice = total.ToString("N0");
    }
}
