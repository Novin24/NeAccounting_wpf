using DomainShared.Extension;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using NeApplication.Services;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class BillPage : INavigableView<BillListViewModel>
    {
        public BillListViewModel ViewModel { get; }
        private readonly ISnackbarService _snackbarService;
        private readonly IPrintServices _printServices;
        public BillPage(BillListViewModel viewModel, ISnackbarService snackbarService, IPrintServices printServices)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
            _printServices = printServices;
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.ChangePageCommand.ExecuteAsync(null);
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var us = ((SuggestBoxViewModel<Guid, long>)args.SelectedItem);
            ViewModel.CusId = us.Id;
            ViewModel.PersonelId = us.UniqNumber;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

        private async void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            var (list, isSuccess) = await ViewModel.PrintInvoices();
            if (!isSuccess)
                return;
            if (!list.Any())
            {
                _snackbarService.Show("خطا", "در بازه انتخابی موردی برای نمایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            var cus = ViewModel.Cuslist.First(t => t.Id == ViewModel.CusId);
            Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"({cus.UniqNumber}) _ {cus.DisplayName}"},
                {"Start_Date",$"{Dtp_Start.DisplayDate}"},
                {"End_Date",$"{Dtp_End.DisplayDate}"},
                {"PrintTime",DateTime.Now.ToShamsiDate(new PersianCalendar()) },
                {"Total_Debt",list.Select(p => p.Bed).Sum().ToString("N0")},
                {"Total_Credit",list.Select(p => p.Bes).Sum().ToString("N0")},
                {"Total_LeftOVver",list.Last().LeftOver.ToString("N0")},
                {"TotalSLeftOver",list.Last().LeftOver.ToString().NumberToPersianString()},
                {"Status",$"{list.Last().Status}"}};

            _printServices.PrintInvoice(@"Reports\ReportInvoices.mrt", "InvoiceListDtos", list, dic);
        }
    }
}
