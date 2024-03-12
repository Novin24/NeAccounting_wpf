using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Diagnostics;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class BillListViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public BillListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
        }


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

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
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
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, false, CurrentPage);
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
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate.Value, EndDate.Value, Desc, CusId.Value, LeftOver, false, CurrentPage);
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
                    _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }


                // اول باید موجودی انبار نسبت به فاکتور خرید یا فروش بازگردانی شود سپس فاکتور حذف شود



                //using UnitOfWork db = new();
                //var isSuccess = await db.DocumentManager.DeleteAsync<Guid>(parameter);
                //if (!isSuccess)
                //{
                //    _snackbarService.Show("کاربر گرامی", "خطا دراتصال به پایگاه داده!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                //    return;
                //}
                //_snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                //await OnSearchInvoice();
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
                        PayTypeEnum = PaymentType.CardToCard.ToDictionary(),
                        PayTypeId = (byte)itme.Type,
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
                        PayTypeEnum = PaymentType.CardToCard.ToDictionary(),
                        //PayTypeEnum = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>(),
                        PayTypeId = (byte)item.Type,
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

                    //var (isSuccess, itm) = await db.DocumentManager.GetSellInvoiceDetail(parameter);
                    //if (!isSuccess)
                    //{
                    //    _snackbarService.Show("خطا", "فاکتور مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    //    return;
                    //}
                    //var mat = await db.MaterialManager.GetMaterails();
                    //var stu = await db.DocumentManager.GetStatus(itm.CustomerId);
                    //var oc = new System.Collections.ObjectModel.ObservableCollection<RemittanceListViewModel>();
                    //int i = 1;
                    //foreach (var it in itm.RemList)
                    //{
                    //    it.RowId = i++;
                    //    it.MatName = mat.First(t => t.Id == it.MaterialId).MaterialName;
                    //    it.UnitName = mat.First(t => t.Id == it.MaterialId).UnitName;
                    //    oc.Add(it);
                    //}
                    //long stx = itm.TotalPrice;
                    //if (itm.CommissionPrice.HasValue && itm.CommissionPrice.Value != 0)
                    //{
                    //    stx -= itm.CommissionPrice.Value;
                    //}
                    //var context = new UpdateSellInvoicePage(new UpdateSellInvoiceViewModel(_snackbarService, _navigationService)
                    //{
                    //    MatList = mat,
                    //    CusName = Cuslist.First(t => t.Id == itm.CustomerId).DisplayName,
                    //    CusNumber = Cuslist.First(t => t.Id == itm.CustomerId).UniqNumber,
                    //    Status = stu.Status,
                    //    Debt = stu.Debt,
                    //    Credit = stu.Credit,
                    //    SubmitDate = itm.Date,
                    //    StaticList = itm.RemList,
                    //    RemainPrice = stx.ToString("N0"),
                    //    InvDescription = itm.InvoiceDescription,
                    //    Commission = itm.Commission,
                    //    LastInvoice = itm.Serial,
                    //    List = oc,
                    //    TotalPrice = itm.TotalPrice.ToString("N0"),
                    //    Totalcommission = itm.Commission.HasValue ? (itm.TotalPrice * (itm.Commission.Value / 100)).ToString("N0") : "0",
                    //    InvoiceId = parameter
                    //}, _snackbarService);

                    break;
                case DocumntType.BuyInv:
                    break;
                case DocumntType.Cheque:
                    break;
                default:
                    break;
            }

            //Type? pageType = NameToPageTypeConverter.Convert("UpdateCustomer");

            //if (pageType == null)
            //{
            //    return;
            //}
            //var servise = _navigationService.GetNavigationControl();

            //var cus = List.First(t => t.Id == parameter);

            //var context = new UpdateCustomerPage(new UpdateCustomerViewModel(_snackbarService, _navigationService)
            //{
            //    Id = cus.Id,
            //    FullName = cus.Name,
            //    Seller = cus.Seller,
            //    Buyer = cus.Buyer,
            //    Address = cus.Address,
            //    CashCredit = cus.CashCredit,
            //    ChequeCredit = cus.ChequeCredit,
            //    TotalCredit = cus.TotalCredit,
            //    PromissoryNote = cus.PromissoryNote,
            //    HavePromissoryNote = cus.HavePromissoryNote,
            //    CusType = (byte)cus.CusType,
            //    HaveCashCredit = cus.HaveCashCredit,
            //    Mobile = cus.Mobile,
            //    NationalCode = cus.NationalCode
            //});

            //servise.Navigate(pageType, context);
        }
    }
}
