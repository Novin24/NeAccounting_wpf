using DomainShared.Extension;
using DomainShared.ViewModels;
using NeAccounting.Models;
using NeAccounting.ViewModels;
using NeApplication.Services;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Invoicedetails.xaml
    /// </summary>
    public partial class InvoicedetailsPage : INavigableView<InvoicedetailsViewModel>
    {
        public InvoicedetailsViewModel ViewModel { get; }
        private readonly IPrintServices _printServices;
        private readonly ISnackbarService _snackbarService;
        public InvoicedetailsPage(InvoicedetailsViewModel viewModel, IPrintServices printServices, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
            _printServices = printServices;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var us = ((SuggestBoxViewModel<Guid, long>)args.SelectedItem);
            ViewModel.CusId = us.Id;
            lbl_personelId.Text = us.UniqNumber.ToString();
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.ChangePageCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task OnPrintList()
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
            var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
            if (printInfo == null)
            {
                _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
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
                {"Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{printInfo.Tabligh}"},
                {"Status",$"{list.Last().Status}"}};

            _printServices.PrintInvoice(@"Required\Reports\ReportRem.mrt", "DetailListDtos", list, dic);
        }

    }
}
