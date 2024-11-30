using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Extension;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Models;
using NeAccounting.Views.Pages;
using NeApplication.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class BillListViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private bool _isInitialized = false;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        private readonly IPrintServices _printServices;

        public BillListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService, IPrintServices printServices)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
            _printServices = printServices;
        }

        #region Properties

        [ObservableProperty]
        private long? _personelId;

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private Guid? _cusId;

        [ObservableProperty]
        private string _desc = "";

        [ObservableProperty]
        private DateTime? _startDate = DateTime.Now;

        [ObservableProperty]
        private DateTime? _endDate = DateTime.Now;

        /// <summary>
        /// به احتساب مانده قبلی
        /// </summary>
        [ObservableProperty]
        private bool _leftOver = true;

        /// <summary>
        /// لیست مشتری ها
        /// </summary>
        [ObservableProperty]
        private List<SuggestBoxViewModel<Guid, long>> _cuslist;

        /// <summary>
        /// لیست فاکتور
        /// </summary>
        [ObservableProperty]
        private IEnumerable<InvoiceListDtos> _invList;
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
            Cuslist = await db.CustomerManager.GetDisplayUser(false);
            if (CusId.HasValue && EndDate.HasValue && StartDate.HasValue)
            {
                _isInit = true;
                var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, false, true, CurrentPage);
                CurrentPage = t.CurrentPage;
                InvList = t.Items;
                PageCount = t.PageCount;
                _isInit = false;
            }
            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OnSearchInvoice()
        {

            if (!CusId.HasValue)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            _isInit = true;
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, false, true, CurrentPage);
            CurrentPage = t.CurrentPage;
            InvList = t.Items;
            PageCount = t.PageCount;
            _isInit = false;
        }

        [RelayCommand]
        private async Task OnChangePage()
        {
            if (_isInit)
            {
                return;
            }
            if (!CusId.HasValue)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, false, false, CurrentPage);
            InvList = t.Items;
            PageCount = t.PageCount;
        }

        public async Task<(IEnumerable<InvoiceListDtos> list, bool isSuccess)> PrintInvoices()
        {
            if (!CusId.HasValue)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return (new List<InvoiceListDtos>(), false);
            }
            if (StartDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ شروع"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return (new List<InvoiceListDtos>(), false);
            }

            if (EndDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ پایان"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return (new List<InvoiceListDtos>(), false);
            }
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, true, true, CurrentPage);
            return new(t.Items, true);
        }

        [RelayCommand]
        private async Task OnRemoveDoc(Guid parameter)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "هشدار !!!",
                Content = new TextBlock() { Text = "آیا از حذف سند اطمینان دارید ؟", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                var doc = InvList.FirstOrDefault(x => x.Id == parameter);
                if (doc == null)
                {
                    _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای حذف یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                switch (doc.Type)
                {
                    case DocumntType.PayDoc:
                        using (UnitOfWork db = new())
                        {
                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.RecDoc:
                        using (UnitOfWork db = new())
                        {
                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.SellInv:
                        using (UnitOfWork db = new())
                        {
                            #region GetDoc
                            var (isSucces, itm) = await db.DocumentManager.GetSellInvoiceDetail(parameter);
                            if (!isSucces)
                            {
                                _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                return;
                            }

                            var (isEx, li) = await db.DocumentManager.GetRetrunSellInvoiceGoods(parameter);
                            #endregion

                            #region UpdateMaterial
                            foreach (var item in itm.RemList)
                            {
                                var amountOf = isEx ? item.AmountOf - li.Where(t => t.MaterialId == item.MaterialId).Sum(t => t.AmountOf) : item.AmountOf;
                                if (amountOf == 0)
                                    continue;
                                var (errore, isSucess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, amountOf, true, item.Price);
                                if (!isSucess)
                                {
                                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                    db.Dispose();
                                    return;
                                }
                            }
                            #endregion

                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.ReturnFromBuy:
                        using (UnitOfWork db = new())
                        {
                            #region GetDoc
                            var (isSucces, itm) = await db.DocumentManager.GetSellInvoiceDetail(parameter);
                            if (!isSucces)
                            {
                                _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                return;
                            }
                            #endregion

                            #region UpdateMaterial
                            foreach (var item in itm.RemList)
                            {
                                var (errore, isSucess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, true, item.Price);
                                if (!isSucess)
                                {
                                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                    db.Dispose();
                                    return;
                                }
                            }
                            #endregion

                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.BuyInv:
                        using (UnitOfWork db = new())
                        {
                            #region GetDoc
                            var (isSucces, itm) = await db.DocumentManager.GetBuyInvoiceDetail(parameter);
                            if (!isSucces)
                            {
                                _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                return;
                            }

                            var (isEx, li) = await db.DocumentManager.GetRetrunBuyInvoiceGoods(parameter);

                            #endregion

                            #region UpdateMaterial
                            foreach (var item in itm.RemList)
                            {
                                var amountOf = isEx ? item.AmountOf - li.Where(t => t.MaterialId == item.MaterialId).Sum(t => t.AmountOf) : item.AmountOf;
                                if (amountOf == 0)
                                    continue;
                                var (errore, isSucess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, amountOf, false, item.Price);
                                if (!isSucess)
                                {
                                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                    db.Dispose();
                                    return;
                                }
                            }
                            #endregion

                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.ReturnFromSell:
                        using (UnitOfWork db = new())
                        {
                            #region GetDoc
                            var (isSucces, itm) = await db.DocumentManager.GetBuyInvoiceDetail(parameter);
                            if (!isSucces)
                            {
                                _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                return;
                            }
                            #endregion

                            #region UpdateMaterial
                            foreach (var item in itm.RemList)
                            {
                                var (errore, isSucess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false, item.Price);
                                if (!isSucess)
                                {
                                    _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                    db.Dispose();
                                    return;
                                }
                            }
                            #endregion

                            var (isSuccess, e) = await db.DocumentManager.DeleteDocument(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    case DocumntType.Cheque:
                        using (UnitOfWork db = new())
                        {
                            var (e, isSuccess) = await db.DocumentManager.RemoveCheque(parameter);
                            if (!isSuccess)
                            {
                                _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                                db.Dispose();
                                return;
                            }
                            await db.SaveChangesAsync();
                        }
                        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                        await OnSearchInvoice();
                        break;

                    default:
                        _snackbarService.Show("کاربر گرامی", "حذف امکان پذیر نمی‌باشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        break;
                }
            }
        }

        [RelayCommand]
        private async Task OnUpdateDoc(Guid parameter)
        {
            var doc = InvList.FirstOrDefault(x => x.Id == parameter);
            if (doc == null)
            {
                _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            _isInitialized = false;
            using UnitOfWork db = new();
            switch (doc.Type)
            {
                case DocumntType.PayDoc:
                    Type? pagetyp = NameToPageTypeConverter.Convert("UpdatePayDoc");

                    if (pagetyp == null)
                    {
                        return;
                    }
                    var servis = _navigationService.GetNavigationControl();

                    var (isSucces, itme) = await db.DocumentManager.GetDocumentById(parameter);
                    if (!isSucces)
                    {
                        _snackbarService.Show("خطا", "سند مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var st = await db.DocumentManager.GetStatus(itme.CustomerId);
                    var docs = await db.DocumentManager.GetSummaryDocs(itme.CustomerId, DocumntType.PayDoc);

                    var contex = new UpdatePayDocPage(new UpdatePayDocViewModel(_snackbarService, _navigationService)
                    {
                        Status = st.Status,
                        SubmitDate = itme.Date,
                        Description = itme.DocDescription,
                        Discount = itme.Dicount,
                        PayTypeEnum = PaymentType.CardToCard.ToEnumDictionary(),
                        //PayTypeEnum = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>(),
                        PayTypeId = itme.Type,
                        DocId = parameter,
                        DocList = docs,
                        Price = itme.Price,
                        TotalPrice = st.Amount,
                        CusNumber = Cuslist.First(t => t.Id == itme.CustomerId).UniqNumber,
                        CusName = Cuslist.First(t => t.Id == itme.CustomerId).DisplayName
                    });
                    servis.Navigate(pagetyp, contex);
                    break;

                case DocumntType.RecDoc:
                    Type? pagety = NameToPageTypeConverter.Convert("UpdateRecDoc");

                    if (pagety == null)
                    {
                        return;
                    }
                    var servi = _navigationService.GetNavigationControl();

                    var (isSucce, item) = await db.DocumentManager.GetDocumentById(parameter);
                    if (!isSucce)
                    {
                        _snackbarService.Show("خطا", "سند مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var s = await db.DocumentManager.GetStatus(item.CustomerId);
                    var dc = await db.DocumentManager.GetSummaryDocs(item.CustomerId, DocumntType.RecDoc);

                    var conte = new UpdateRecDocPage(new UpdateRecDocViewModel(_snackbarService, _navigationService)
                    {
                        Status = s.Status,
                        SubmitDate = item.Date,
                        Description = item.DocDescription,
                        Discount = item.Dicount,
                        PayTypeEnum = PaymentType.CardToCard.ToEnumDictionary(),
                        //PayTypeEnum = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>(),
                        PayTypeId = item.Type,
                        DocId = parameter,
                        DocList = dc,
                        Price = item.Price,
                        TotalPrice = s.Amount,
                        CusNumber = Cuslist.First(t => t.Id == item.CustomerId).UniqNumber,
                        CusName = Cuslist.First(t => t.Id == item.CustomerId).DisplayName
                    });
                    servi.Navigate(pagety, conte);
                    break;

                case DocumntType.SellInv:
                    Type? pageType = NameToPageTypeConverter.Convert("UpdateSellInvoice");

                    if (pageType == null)
                    {
                        return;
                    }

                    EditInvoiceDetails.InvoiceId = parameter;

                    _navigationService.Navigate(pageType);

                    break;

                case DocumntType.BuyInv:
                    Type? pagType = NameToPageTypeConverter.Convert("UpdateBuyInvoice");

                    if (pagType == null)
                    {
                        return;
                    }

                    EditInvoiceDetails.InvoiceId = parameter;
                    _navigationService.Navigate(pagType);
                    break;

                case DocumntType.ReturnFromSell:
                    Type? pagTyp = NameToPageTypeConverter.Convert("UpdateFromSell");

                    if (pagTyp == null || doc.ParentId == null)
                    {
                        _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var srvis = _navigationService.GetNavigationControl();

                    var (isSuccess, itm) = await db.DocumentManager.GetFromTheSellInvoiceDetail(doc.ParentId.Value, parameter);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    // var stu = await db.DocumentManager.GetStatus(itm.CustomerId);
                    (string error, CustomerListDto cus) = await db.CustomerManager.GetCustomerById(itm.CustomerId);

                    if (!string.IsNullOrEmpty(error))
                    {
                        _snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var cntx = new UpdateFromSellPage(_snackbarService, new UpdateFromTheSellViewModel(_snackbarService, _navigationService, _contentDialogService)
                    {
                        CusName = cus.Name,
                        CusId = cus.Id,
                        ReturndocId = parameter,
                        ParentDocId = doc.ParentId.Value,
                        CusNum = cus.UniqNumber,
                        InvDescription = itm.Description,
                        SubmitDate = itm.Date,
                        TotalPrice = itm.TotalInvPrice.ToString("N0"),
                        MatList = itm.ParentRemList.Select(t => new DomainShared.ViewModels.Pun.MatListDto()
                        {
                            Id = t.MaterialId,
                            IsService = t.IsService,
                            UnitName = t.UnitName,
                            MaterialName = t.MatName,
                            LastBuyPrice = t.Price,
                            LastSellPrice = t.Price
                        }).ToList(),
                        StaticList = itm.ReturnRemList,
                        SellGoods = itm.ParentRemList,
                        List = new ObservableCollection<RemittanceListViewModel>(itm.ReturnRemList),
                        ParentInvoiceSerial = itm.ParentSerial,
                        ReturnInvoicSerial = itm.ReturnSerial,
                    });
                    srvis.Navigate(pagTyp, cntx);

                    break;

                case DocumntType.ReturnFromBuy:
                    Type? pgTyp = NameToPageTypeConverter.Convert("UpdateFromBuy");

                    if (pgTyp == null || doc.ParentId == null)
                    {
                        _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var srvs = _navigationService.GetNavigationControl();

                    var (sc, itmm) = await db.DocumentManager.GetFromTheBuyInvoiceDetail(doc.ParentId.Value, parameter);
                    if (!sc)
                    {
                        _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    //var stuv = await db.DocumentManager.GetStatus(itmm.CustomerId);
                    (string err, CustomerListDto cuss) = await db.CustomerManager.GetCustomerById(itmm.CustomerId);

                    if (!string.IsNullOrEmpty(err))
                    {
                        _snackbarService.Show("خطا", err, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }

                    var cnx = new UpdateFromBuyPage(_snackbarService, new UpdateFromTheBuyViewModel(_snackbarService, _navigationService, _contentDialogService)
                    {
                        CusName = cuss.Name,
                        CusId = cuss.Id,
                        ReturndocId = parameter,
                        ParentDocId = doc.ParentId.Value,
                        CusNum = cuss.UniqNumber,
                        InvDescription = itmm.Description,
                        SubmitDate = itmm.Date,
                        TotalPrice = itmm.TotalInvPrice.ToString("N0"),
                        MatList = itmm.ParentRemList.Select(t => new DomainShared.ViewModels.Pun.MatListDto()
                        {
                            Id = t.MaterialId,
                            IsService = t.IsService,
                            UnitName = t.UnitName,
                            MaterialName = t.MatName,
                            LastBuyPrice = t.Price,
                            LastSellPrice = t.Price
                        }).ToList(),
                        BuyGoods = itmm.ParentRemList,
                        List = new ObservableCollection<RemittanceListViewModel>(itmm.ReturnRemList),
                        StaticList = itmm.ReturnRemList,
                        ParentInvoiceSerial = itmm.ParentSerial,
                        ReturnInvoicSerial = itmm.ReturnSerial,
                    });
                    srvs.Navigate(pgTyp, cnx);

                    break;

                case DocumntType.Cheque:
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private async Task OnPrintList()
        {
            var (list, isSuccess) = await PrintInvoices();
            if (!isSuccess)
                return;
            if (!list.Any())
            {
                _snackbarService.Show("خطا", "در بازه انتخابی موردی برای نمایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var cus = Cuslist.First(t => t.Id == CusId);
            var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
            if (printInfo == null)
            {
                _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            PersianCalendar pc = new();
            Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"({cus.UniqNumber}) _ {cus.DisplayName}"},
                {"Start_Date",StartDate.ToShamsiDate(pc)},
                {"End_Date",EndDate.ToShamsiDate(pc)},
                {"PrintTime",DateTime.Now.ToShamsiDate(new PersianCalendar()) },
                {"Total_Debt",list.Select(p => p.Bed).Sum().ToString("N0")},
                {"Total_Credit",list.Select(p => p.Bes).Sum().ToString("N0")},
                {"Total_LeftOVver",list.Last().LeftOver.ToString("N0")},
                {"TotalSLeftOver",list.Last().LeftOver.ToString().NumberToPersianString()},
                {"Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{printInfo.Tabligh}"},
                {"Status",$"{list.Last().Status}"}};

            _printServices.PrintInvoice(@"Required\Reports\ReportInvoices.mrt", "InvoiceListDtos", list, dic);
        }

        [RelayCommand]
        private async Task OnReturnGoods(Guid parameter)
        {
            var doc = InvList.FirstOrDefault(x => x.Id == parameter);
            if (doc == null)
            {
                _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            using UnitOfWork db = new();
            if (doc.Type == DocumntType.SellInv)
            {
                Type? pagetyp = NameToPageTypeConverter.Convert("FromTheSell");

                if (pagetyp == null)
                {
                    return;
                }
                var servis = _navigationService.GetNavigationControl();

                var (isSuccess, itm) = await db.DocumentManager.GetSellInvoiceDetail(parameter);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                //var stu = await db.DocumentManager.GetStatus(itm.CustomerId);
                (string error, CustomerListDto cus) = await db.CustomerManager.GetCustomerById(itm.CustomerId);

                if (!string.IsNullOrEmpty(error))
                {
                    _snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                var contex = new FromTheSellPage(_snackbarService, new FromTheSellViewModel(_snackbarService, _navigationService, _contentDialogService)
                {
                    CusName = cus.Name,
                    CusId = cus.Id,
                    DocId = parameter,
                    CusNum = cus.UniqNumber,
                    MatList = itm.RemList.Select(t => new DomainShared.ViewModels.Pun.MatListDto()
                    {
                        Id = t.MaterialId,
                        IsService = t.IsService,
                        UnitName = t.UnitName,
                        MaterialName = t.MatName,
                        LastBuyPrice = t.Price,
                        LastSellPrice = t.Price
                    }).ToList(),
                    SellGoods = itm.RemList,
                    LastInvoice = itm.Serial,
                });
                servis.Navigate(pagetyp, contex);
            }
            else
            {
                Type? pagetyp = NameToPageTypeConverter.Convert("FromTheBuy");

                if (pagetyp == null)
                {
                    return;
                }
                var servis = _navigationService.GetNavigationControl();

                var (isSuccess, itm) = await db.DocumentManager.GetBuyInvoiceDetail(parameter);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                //var stu = await db.DocumentManager.GetStatus(itm.CustomerId);
                (string error, CustomerListDto cus) = await db.CustomerManager.GetCustomerById(itm.CustomerId);

                if (!string.IsNullOrEmpty(error))
                {
                    _snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }

                var contex = new FromTheBuyPage(_snackbarService, new FromTheBuyViewModel(_snackbarService, _navigationService, _contentDialogService)
                {
                    CusName = cus.Name,
                    CusId = cus.Id,
                    DocId = parameter,
                    CusNum = cus.UniqNumber,
                    MatList = itm.RemList.Select(t => new DomainShared.ViewModels.Pun.MatListDto()
                    {
                        Id = t.MaterialId,
                        IsService = t.IsService,
                        UnitName = t.UnitName,
                        MaterialName = t.MatName,
                        LastBuyPrice = t.Price,
                        LastSellPrice = t.Price
                    }).ToList(),
                    BuyGoods = itm.RemList,
                    LastInvoice = itm.Serial,
                });
                servis.Navigate(pagetyp, contex);
            }
        }
        #endregion
    }
}



