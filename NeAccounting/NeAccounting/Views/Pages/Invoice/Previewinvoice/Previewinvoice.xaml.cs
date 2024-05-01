using DomainShared.Errore;
using DomainShared.Extension;
using DomainShared.ViewModels.Pun;
using NeAccounting.Models;
using NeApplication.Services;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Previewinvoice.xaml
    /// </summary>
    public partial class PreviewinvoicePage : INavigableView<PreviewinvoiceViewModel>
    {
        public PreviewinvoiceViewModel ViewModel { get; }
        private readonly IPrintServices _printServices;
        private readonly ISnackbarService _snackbarService;
        private double _totalEntity;
        private long _price;
        public PreviewinvoicePage(PreviewinvoiceViewModel viewModel, ISnackbarService snackbarService, IPrintServices printServices)
        {
            ViewModel = viewModel;
            DataContext = this;
            _snackbarService = snackbarService;
            _printServices = printServices;
            InitializeComponent();
            Txt_name.Focus();
        }

        [RelayCommand]
        private void OnAddRow()
        {
            Btn_submit.Focus();
            if (ViewModel.OnAdd())
            {
                ViewModel.AmountOf = null;
                ViewModel.MaterialId = null;
                ViewModel.Description = null;
                ViewModel.MatPrice = null;
                txt_MaterialName.Text = string.Empty;
                txt_UnitName.Text = string.Empty;
                txt_Unit_price.Text = string.Empty;
                txt_total_price.Text = string.Empty;
                txt_UnitDescription.Text = string.Empty;
                txt_MaterialName.Focus();
            }
            dgv_Inv.Items.Refresh();
        }

        private void Txt_mat_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var mat = (MatListDto)args.SelectedItem;
            ViewModel.MaterialId = mat.Id;
            ViewModel.MatPrice = mat.LastSellPrice;
            _totalEntity = mat.Entity;
            txt_UnitName.Text = mat.UnitName;
            txt_Unit_price.Text = mat.LastSellPrice.ToString("N0");
            _price = mat.LastSellPrice;
        }

        private void Txt_amount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not NumberBox nb)
                return;

            if (nb.Value == null)
                return;

            txt_total_price.Text = (nb.Value.Value * _price).ToString("N0");
        }

        private void Txt_Unit_price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }

        private void Txt_Unit_price_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not TextBox txt_price)
                return;

            txt_price.Text = txt_price.Text.Replace(" ", "000");

            if (txt_price.Text == "" || txt_price.Text == "0") return;
            CultureInfo culture = new("en-US");
            long valueBefore = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);
            _price = valueBefore;
            txt_price.Text = String.Format(culture, "{0:N0}", valueBefore);
            txt_price.Select(txt_price.Text.Length, 0);
        }

        private void Txt_Unit_price_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox txt_price)
                return;

            if (ViewModel.AmountOf == null)
                return;


            ViewModel.MatPrice = _price = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);

            txt_total_price.Text = (ViewModel.AmountOf.Value * _price).ToString("N0");
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation())
            {
                _snackbarService.Show("اخطار", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            if (!Validation())
            {
                _snackbarService.Show("اخطار", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ویرایش مجدد نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            int id = int.Parse(btn.Tag.ToString());
            var (s, itm) = ViewModel.OnUpdate(id);
            if (!s) return;
            txt_MaterialName.Text = itm.MatName;
            txt_total_price.Text = itm.TotalPrice.ToString("N0");
            txt_UnitName.Text = itm.UnitName;
            _price = itm.Price;
            txt_Unit_price.Text = itm.Price.ToString();
            dgv_Inv.Items.Refresh();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            ViewModel.OnRemove(id);
            dgv_Inv.Items.Refresh();
        }

        private bool Validation()
        {
            if (txt_MaterialName.Text != string.Empty)
                return false;

            if (txt_amount.Value != null && txt_amount.Value != 0)
                return false;

            if (txt_Unit_price.Text != string.Empty)
                return false;

            return true;
        }

        [RelayCommand]
        private void OnSubmit()
        {
            if (string.IsNullOrEmpty(ViewModel.CusName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام مشتری"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (ViewModel.SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (!Validation())
            {
                _snackbarService.Show("اخطار", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به پرینت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            var printInfo = JsonConvert.DeserializeObject<PrintInfo>(File.ReadAllText(@"Required\Reports\PrintInfo.json"));
            if (printInfo == null)
            {
                _snackbarService.Show("خطا", "فایل پرینت یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            PersianCalendar pc = new();
            long total = ViewModel.List.Sum(t => t.TotalPrice);
            Dictionary<string, string> dic = new()
            {
                {"Customer_Name",$"{ViewModel.CusName}"},
                {"PrintTime",ViewModel.SubmitDate.ToShamsiDate(pc)},
                {"Total_InvoicePrice",ViewModel.TotalPrice},
                {"TotalSLeftOver",total.ToString().NumberToPersianString()},
                { "Management",$"{printInfo.Management}"},
                {"Company_Name",$"{printInfo.Company_Name}"},
                {"Tabligh",$"{printInfo.Tabligh}"}
            };

            _printServices.PrintInvoice(@"Required\Reports\Pishfactor.mrt", "DetailListDtos", ViewModel.List, dic);
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();
    }
}
