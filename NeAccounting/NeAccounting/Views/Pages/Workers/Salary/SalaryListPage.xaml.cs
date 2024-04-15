using Common.Utilities;
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
    /// Interaction logic for SalaryListPage.xaml
    /// </summary>
    public partial class SalaryListPage : INavigableView<SalaryListViewModel>
    {
        public SalaryListViewModel ViewModel { get; }
        private readonly IPrintServices _printServices;
        private readonly ISnackbarService _snackbarService;

        public SalaryListPage(SalaryListViewModel viewModel, IPrintServices printServices, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _printServices = printServices;
            _snackbarService = snackbarService;
        }

        private void autoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
        }

        private async void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            var id = int.Parse(btn.Tag.ToString());
            var (s, i) = await ViewModel.PrintSalary(id);
            if (!s) return;
            var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
            if (printInfo == null)
            {
                _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            PersianCalendar pc = new();
            Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"({i.PersonelId}) _ {i.WorkerName}"},
                {"PrintTime",DateTime.Now.ToShamsiDate(pc) },
                {"Total_InvoicePrice",i.LeftOver.ToString("N0")},
                {"AmountOf",i.AmountOf.ToString("N0")},
                {"OverTime",i.OverTime.ToString("N0")},
                {"Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{i.SubmitYear}  {i.SubmitMonth.ToPersianMonth()}"},
                {"RightHousingAndFood",i.RightHousingAndFood.ToString("N0")},
                {"ChildAllowance",i.ChildAllowance.ToString("N0")},
                {"FinancialAid",i.FinancialAid.ToString("N0")},
                {"Insurance",i.Insurance.ToString("N0")},
                {"LoanInstallment",i.LoanInstallment.ToString("N0")},
                {"Tax",i.Tax.ToString("N0")},
                {"OtherAdditions",i.OtherAdditions.ToString("N0")},
                {"OtherDeductions",i.OtherDeductions.ToString("N0")},
                {"Total_Additions",(i.AmountOf + i.OverTime + i.RightHousingAndFood + i.ChildAllowance + i.OtherAdditions).ToString("N0")},
                {"Total_Deductions",(i.Insurance + i.Tax + i.FinancialAid + i.LoanInstallment+ i.OtherDeductions).ToString("N0")},
                {"FunctionNum",i.FunctionNum.ToString()},
                {"OverTimeNum",i.OverTimeNum.ToString()},
                {"TotalSLeftOver",i.LeftOver.ToString().NumberToPersianString()},
            };

            _printServices.PrintInvoice(@"Required\Reports\SalaryReport.mrt", dic);
        }
    }
}
