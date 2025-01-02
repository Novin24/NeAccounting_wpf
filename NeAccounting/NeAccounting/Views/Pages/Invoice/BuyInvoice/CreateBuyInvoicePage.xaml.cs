using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateBuyInvoicePage.xaml
    /// </summary>
    public partial class CreateBuyInvoicePage : INavigableView<CreateBuyInvoiceViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreateBuyInvoiceViewModel ViewModel { get; }
        private double _totalEntity;
        private long _price;
        public CreateBuyInvoicePage(CreateBuyInvoiceViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
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
				lbl_matEntity.Text = string.Empty;
				txt_MaterialName.Focus();
            }
            dgv_Inv.Items.Refresh();
        }

        private async void Txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
            txt_Credit.Text = user.TotalValidity.ToString("N0");
            await ViewModel.OnSelectCus(user.Id);
        }

        private void Txt_mat_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var mat = (MatListDto)args.SelectedItem;
            ViewModel.MaterialId = mat.Id;
            ViewModel.MatPrice = mat.LastBuyPrice;
            _totalEntity = mat.Entity;
            txt_UnitName.Text = mat.UnitName;
            txt_amount.Text = "0";
			lbl_matEntity.Text = mat.Entity.ToString("N0");
			txt_Unit_price.Text = mat.LastBuyPrice.ToString("N0");
            _price = mat.LastBuyPrice;
        }

        private void Txt_amount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not NumberBox nb)
                return;

            if (nb.Value == null)
                return;

            //if (nb.Value > _totalEntity)
            //{
            //    _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
            //}
            txt_total_price.Text = (nb.Value.Value * _price).ToString("N0");
        }

        private void Txt_Unit_price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }

        private void Txt_Unit_price_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not TextBox txt_price) return;

            txt_price.Text = txt_price.Text.Replace(" ", "000");

            if (txt_price.Text == "" || txt_price.Text == "0") return;
            CultureInfo culture = new("en-US");
            long valueBefore = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);
            _price = valueBefore;
            txt_price.Text = String.Format(culture, "{0:N0}", valueBefore);
            txt_price.CaretIndex = txt_price.Text.Length;
        }

        private void Txt_Unit_price_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox txt_price)
                return;

            if (ViewModel.AmountOf == null)
                return;

            if (string.IsNullOrEmpty(txt_price.Text))
            {
                _snackbarService.Show("اخطار", "وارد کردن مبلغ واحد الزامیست!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            ViewModel.MatPrice = _price = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);

            txt_total_price.Text = (ViewModel.AmountOf.Value * _price).ToString("N0");
        }
        [RelayCommand]
        private async Task OnSubmit()
        {
            Btn_submit.Focus();
            if (!Validation())
            {
                _snackbarService.Show("اخطار", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (await ViewModel.OnSumbit())
            {
                txt_CustomerName.Text = string.Empty;
                txt_MaterialName.Text = string.Empty;
                txt_UnitName.Text = string.Empty;
                txt_Credit.Text = "0";
                txt_Unit_price.Text = string.Empty;
                txt_total_price.Text = string.Empty;
                txt_UnitDescription.Text = string.Empty;
                lbl_cusId.Text = string.Empty;
            }
            await ViewModel.LoadMaterialList();
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


        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_CustomerName.Focus();
        }
    }
}
