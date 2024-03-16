using DomainShared.ViewModels.Customer;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CustomerListViewModel : ObservableObject, INavigationAware
    {

        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;

        public CustomerListViewModel(INavigationService navigationService, IContentDialogService contentDialogService, ISnackbarService snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
        }


        [ObservableProperty]
        private string _nationalCode = "";

        [ObservableProperty]
        private string _mobile = "";

        [ObservableProperty]
        private string _name = "";

        [ObservableProperty]
        private IEnumerable<CustomerListDto> _list;

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
            List = await db.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
        }

        [RelayCommand]
        public async Task OnSearchCus()
        {
            using UnitOfWork db = new();
            List = await db.CustomerManager.GetCustomerList(Name, NationalCode, Mobile);
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

        //private async Task OnArchiveCus(Guid parameter, bool isArchive)
        //{
        //    var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
        //    {
        //        Title = "آیا از بایگانی اطمینان دارید!!!",
        //        Content = Application.Current.Resources["DeleteDialogContent"],
        //        PrimaryButtonText = "بله",
        //        SecondaryButtonText = "خیر",
        //        CloseButtonText = "انصراف",
        //    });

        //    if (result == ContentDialogResult.Primary)
        //    {
        //        using UnitOfWork db = new();
        //        var (e, s) = await db.CustomerManager.ArchiveCustomer(parameter, isArchive);
        //        if (!s)
        //        {
        //            _snackbarService.Show("کاربر گرامی", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
        //            return;
        //        }
        //        _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

        //        await OnSearchCus();
        //    }
        //}

        [RelayCommand]
        private void OnUpdateCus(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateCustomer");

            if (pageType == null)
            {
                return;
            }

            var cus = List.First(t => t.Id == parameter);

            var context = new UpdateCustomerPage(new UpdateCustomerViewModel(_snackbarService, _navigationService)
            {
                Id = cus.Id,
                FullName = cus.Name,
                Seller = cus.Seller,
                Buyer = cus.Buyer,
                Address = cus.Address,
                CashCredit = cus.CashCredit,
                ChequeCredit = cus.ChequeCredit,
                TotalCredit = cus.TotalCredit,
                PromissoryNote = cus.PromissoryNote,
                HavePromissoryNote = cus.HavePromissoryNote,
                CusType = (byte)cus.CusType,
                HaveCashCredit = cus.HaveCashCredit,
                Mobile = cus.Mobile,
                NationalCode = cus.NationalCode
            });

            var servise = _navigationService.GetNavigationControl();
            servise.Navigate(pageType, context);
        }

        [RelayCommand]
        private void OnAddGaranteeCheque(Guid parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("AddCheque");

            if (pageType == null)
            {
                return;
            }
            var servise = _navigationService.GetNavigationControl();

            //var cus = List.First(t => t.Id == parameter);

            //var context = new AddChequePage(new Pages.AddChequeViewModel(_snackbarService, _navigationService)
            //{

            //});

            //servise.Navigate(pageType, context);
        }
        [RelayCommand]
        private async Task OnActive(Guid id)
        {
            using UnitOfWork db = new();
            await db.CustomerManager.ArchiveCustomer(id, true);
            await db.SaveChangesAsync();
            List = await db.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
        }
        [RelayCommand]
        private async Task OnDeActive(Guid id)
        {
            using UnitOfWork db = new();
            await db.CustomerManager.ArchiveCustomer(id, false);
            await db.SaveChangesAsync();
            List = await db.CustomerManager.GetCustomerList(string.Empty, string.Empty, string.Empty);
        }
    }
}
