using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
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
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private DateTime _endDate = DateTime.Now;

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
        private IEnumerable<InvoiceListDto> _invList;

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
            Cuslist = await db.CustomerManager.GetDisplayUser();
        }

        [RelayCommand]
        public async Task OnSearchInvoice()
        {
            if (!CusId.HasValue)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));

                return;
            }
            using UnitOfWork db = new();
            var t = await db.DocumentManager.GetInvoicesByDate(StartDate, EndDate, Desc, CusId.Value, LeftOver, false, CurrentPage, 3);
            InvList = t.Items;
            PageCount = t.PageCount;
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
        private void OnUpdateDoc(Guid parameter)
        {
            var doc = InvList.FirstOrDefault(x => x.Id == parameter);
            if (doc == null)
            {
                _snackbarService.Show("کاربر گرامی", "ردیف مورد نظر برای ویرایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            switch (doc.Type)
            {
                case DomainShared.Enums.DocumntType.Pay:
                    break;
                case DomainShared.Enums.DocumntType.Rec:
                    break;
                case DomainShared.Enums.DocumntType.SellInv:
                    break;
                case DomainShared.Enums.DocumntType.BuyInv:
                    break;
                case DomainShared.Enums.DocumntType.Cheque:
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
