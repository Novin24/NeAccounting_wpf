using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static Stimulsoft.Client.Designer.Components.StiCloudPageSetupWindow;

namespace NeAccounting.ViewModels
{
    public partial class DalyBookViewModel(ISnackbarService snackbarService) : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService = snackbarService;

        /// <summary>
        /// لیست فاکتور
        /// </summary>
        [ObservableProperty]
        private IEnumerable<DalyBookDto> _invList = [];

        [ObservableProperty]
        private int _pageCount = 1;

        [ObservableProperty]
        private DateTime? _date = DateTime.Now;

        [ObservableProperty]
        private int _currentPage = 1;

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
            var result = await db.DocumentManager.GetDalyBook(Date.Value, CurrentPage);
            PageCount = result.PageCount;
            CurrentPage = result.CurrentPage;
            InvList = result.Items;
        }

        [RelayCommand]
        public async Task OnSearch()
        {
            if (Date == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetDalyBook(Date.Value, CurrentPage);
            CurrentPage = t.CurrentPage;
            PageCount = t.PageCount;
            InvList = t.Items;
        }


        [RelayCommand]
        private async Task OnPageChenge()
        {
            if (Date == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using UnitOfWork db = new();
            var result = await db.DocumentManager.GetDalyBook(Date.Value, CurrentPage);
            PageCount = result.PageCount;
            InvList = result.Items;
        }

        //[RelayCommand]
        //private async Task OnRemoveDoc(Guid parameter)
        //{
        //    var result = await _contentDialogService.ShowSimpleDialogAsync(
        //    new SimpleContentDialogCreateOptions()
        //    {
        //        Title = "آیا از حذف اطمینان دارید!!!",
        //        Content = Application.Current.Resources["DeleteDialogContent"],
        //        PrimaryButtonText = "بله",
        //        SecondaryButtonText = "خیر",
        //        CloseButtonText = "انصراف",
        //    });

        //    if (result == ContentDialogResult.Primary)
        //    {
        //        var doc = InvList.FirstOrDefault(x => x.Id == parameter);
        //        if (doc == null)
        //        {
        //            _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //            return;
        //        }


        //        // اول باید موجودی انبار نسبت به فاکتور خرید یا فروش بازگردانی شود سپس فاکتور حذف شود



        //        //using UnitOfWork db = new();
        //        //var isSuccess = await db.DocumentManager.DeleteAsync<Guid>(parameter);
        //        //if (!isSuccess)
        //        //{
        //        //    _snackbarService.Show("کاربر گرامی", "خطا دراتصال به پایگاه داده!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //        //    return;
        //        //}
        //        //_snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        //        //await OnSearchInvoice();
        //    }
        //}

        //[RelayCommand]
        //private void OnUpdateDoc(Guid parameter)
        //{
        //    var doc = InvList.FirstOrDefault(x => x.Id == parameter);
        //    if (doc == null)
        //    {
        //        _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //        return;
        //    }

        //    switch (doc.Type)
        //    {
        //        case DomainShared.Enums.DocumntType.PayDoc:
        //            break;
        //        case DomainShared.Enums.DocumntType.RecDoc:
        //            break;
        //        case DomainShared.Enums.DocumntType.SellInv:
        //            break;
        //        case DomainShared.Enums.DocumntType.BuyInv:
        //            break;
        //        case DomainShared.Enums.DocumntType.Cheque:
        //            break;
        //        default:
        //            break;
        //    }

        //    //Type? pageType = NameToPageTypeConverter.Convert("UpdateCustomer");

        //    //if (pageType == null)
        //    //{
        //    //    return;
        //    //}
        //    //var servise = _navigationService.GetNavigationControl();

        //    //var cus = List.First(t => t.Id == parameter);

        //    //var context = new UpdateCustomerPage(new UpdateCustomerViewModel(_snackbarService, _navigationService)
        //    //{
        //    //    Id = cus.Id,
        //    //    FullName = cus.Name,
        //    //    Seller = cus.Seller,
        //    //    Buyer = cus.Buyer,
        //    //    Address = cus.Address,
        //    //    CashCredit = cus.CashCredit,
        //    //    ChequeCredit = cus.ChequeCredit,
        //    //    TotalCredit = cus.TotalCredit,
        //    //    PromissoryNote = cus.PromissoryNote,
        //    //    HavePromissoryNote = cus.HavePromissoryNote,
        //    //    CusType = (byte)cus.CusType,
        //    //    HaveCashCredit = cus.HaveCashCredit,
        //    //    Mobile = cus.Mobile,
        //    //    NationalCode = cus.NationalCode
        //    //});

        //    //servise.Navigate(pageType, context);
        //}
    }
}
