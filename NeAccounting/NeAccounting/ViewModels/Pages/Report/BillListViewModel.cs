using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class BillListViewModel : ObservableObject, INavigationAware
    {
        private bool _isInit;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public BillListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
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
        private bool _leftOver;

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
            Cuslist = await db.CustomerManager.GetDisplayUser(true);
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
                Title = "آیا از حذف اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
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
                            var (e,isSuccess) = await db.DocumentManager.RemoveCheque(parameter);
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
                        TotalPrice = Math.Abs(st.Amount).ToString("N0"),
                        TotalPricee = Math.Abs(st.Amount),
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
                        TotalPrice = Math.Abs(s.Amount).ToString("N0"),
                        TotalPricee = Math.Abs(s.Amount),
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

                case DocumntType.Cheque:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}



