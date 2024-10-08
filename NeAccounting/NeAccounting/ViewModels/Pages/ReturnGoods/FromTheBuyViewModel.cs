﻿using DomainShared.Constants;
using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class FromTheBuyViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService) : ObservableObject
{
    private readonly ISnackbarService _snackbarService = snackbarService;
    private readonly INavigationService _navigationService = navigationService;
    private readonly IContentDialogService _contentDialogService = contentDialogService;
    private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;



    private int rowId = 1;

    /// <summary>
    /// لیست اجناس  فاکتور خرید
    /// </summary>
    [ObservableProperty]
    private List<RemittanceListViewModel> _buyGoods;

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
    /// شماره مشتری
    /// </summary>
    [ObservableProperty]
    private long _CusNum;

    /// <summary>
    /// شناسه فاکتور
    /// </summary>
    [ObservableProperty]
    private Guid _docId;


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
    /// شماره فاکتور
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

        if (_isreadonly)
        {
            _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
            return false;
        }
        if (MaterialId == null)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        if (MatPrice == null || MatPrice == 0)
        {
            _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
            return false;
        }

        long maxPrice = BuyGoods.Where(c => c.MaterialId == MaterialId).Max(t => t.Price);
        if (MatPrice > maxPrice)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = $"مبلغ وارد شده بیشتر از قیمت خرید ({maxPrice:N0}) می‌باشد \n آیا ادامه میدهید!!!", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
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

        double totalAmountOf = BuyGoods.Where(c => c.MaterialId == MaterialId).Sum(t => t.AmountOf);
        double listAmountOf = List.Where(c => c.MaterialId == MaterialId).Sum(t => t.AmountOf);
        if ((listAmountOf + AmountOf) > totalAmountOf)
        {
            var unitName = MatList.First(c => c.Id == MaterialId).UnitName;
            _snackbarService.Show("خطا", $"مقدار وارد شده بیشتر از مقدار خرید ({totalAmountOf} _ {unitName})", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
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
            Price = MatPrice.Value,
            RowId = rowId,
            TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
            Description = Description,
            MaterialId = MaterialId.Value,
        });
        long total = List.Sum(t => t.TotalPrice);
        TotalPrice = total.ToString("N0");
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
            long total = List.Sum(t => t.TotalPrice);
            TotalPrice = total.ToString("N0");
            RefreshRow(ref rowId);
        }
    }

    /// <summary>
    /// ثبت فاکتور
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task OnSubmit()
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
        #endregion

        #region UpdateMaterial
        using UnitOfWork db = new();
        foreach (var item in List)
        {
            if (item.IsService) continue;

            var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false);
            if (!isSuccess)
            {
                _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
        }
        #endregion

        #region CreateBuylDoc
        var totalInvoicePrice = List.Sum(t => t.TotalPrice);

        var (e, s) = await db.DocumentManager.ReturnFromBuy(DocId, CusId, totalInvoicePrice, InvDescription, SubmitDate.Value, List);
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

}
