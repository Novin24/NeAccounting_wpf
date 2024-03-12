using DomainShared.ViewModels.Pun;
using DomainShared.ViewModels;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;
using DomainShared.ViewModels.Document;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateBuyInvoicePage.xaml
    /// </summary>
    public partial class UpdateBuyInvoicePage : INavigableView<UpdateBuyInviceViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public UpdateBuyInviceViewModel ViewModel { get; }
        private double _totalEntity;
        private long _price;
        public UpdateBuyInvoicePage()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            txt_MaterialName.Text = string.Empty;
            txt_MaterialName.Focus();
            dgv_Inv.Items.Refresh();
        }


        private void Txt_mat_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var mat = (MatListDto)args.SelectedItem;
            lbl_matId.Text = mat.Id.ToString();
            ViewModel.MatPrice = mat.LastSellPrice;
            _totalEntity = mat.Entity;
            txt_UnitName.Text = mat.UnitName;
            txt_Unit_price.Text = mat.LastSellPrice.ToString("N0");
            _price = mat.LastSellPrice;
            lbl_MatPrice.Text = _price.ToString();
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

            if (txt_amount.Value == null || txt_amount.Value == 0)
                return;


            _price = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);
            lbl_MatPrice.Text = _price.ToString();

            txt_total_price.Text = (txt_amount.Value.Value * _price).ToString("N0");
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
            var mat = dgv_Inv.ItemsSource.Cast<RemittanceListViewModel>().FirstOrDefault(t => t.RowId == id);
            if (mat == null)
                return;
            txt_MaterialName.Text = mat.MatName;
            dgv_Inv.Items.Refresh();
        }


        private bool Validation()
        {
            if (!string.IsNullOrEmpty(txt_MaterialName.Text.Trim()))
                return false;

            if (txt_amount.Value != null && txt_amount.Value != 0)
                return false;

            if (!string.IsNullOrEmpty(txt_Unit_price.Text.Trim()))
                return false;

            return true;
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();

    }
}
