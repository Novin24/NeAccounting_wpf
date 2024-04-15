using DomainShared.Extension;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using Infrastructure.UnitOfWork;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
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

        private async void Btn_PrintOneInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            var id = Guid.Parse(btn.Tag.ToString());

            InvoiceDetailUpdateDto invoice;
            using UnitOfWork db = new();
            if (ViewModel.InvList.First(t => t.Id == id).Type == DomainShared.Enums.DocumntType.SellInv)
            {
                var (isSuccess, itm) = await db.DocumentManager.GetSellInvoiceDetail(id);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", "صورتحساب مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                invoice = itm;
            }
            else
            {
                var (isSuccess, itm) = await db.DocumentManager.GetBuyInvoiceDetail(id);
                if (!isSuccess)
                {
                    _snackbarService.Show("خطا", "صورتحساب مورد نظر یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                invoice = itm;
            }

            var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
            if (printInfo == null)
            {
                _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            long leftOverPrice = invoice.CommissionPrice.HasValue && invoice.CommissionPrice.Value != 0 ?
                (invoice.TotalPrice - invoice.CommissionPrice.Value) : invoice.TotalPrice;
            var cus = ViewModel.Cuslist.First(t => t.Id == ViewModel.CusId);
            PersianCalendar pc = new();
            Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"({cus.UniqNumber}) _ {cus.DisplayName}"},
                {"SubmitTime",$"{invoice.Date.ToShamsiDate(pc)}"},
                {"PrintTime",DateTime.Now.ToShamsiDate(pc) },
                {"Total_InvoicePrice",invoice.TotalPrice.ToString("N0")},
                {"Commission",invoice.CommissionSPrice},
                {"LeftOverPrice",leftOverPrice.ToString("N0")},
                {"TotalSLeftOver",leftOverPrice.ToString().NumberToPersianString()},
                {"Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{printInfo.Tabligh}"}
            };

            _printServices.PrintInvoice(@"Required\Reports\ReportOneInvoice.mrt", "DetailListDtos", invoice.RemList, dic);
        }
    }
}
