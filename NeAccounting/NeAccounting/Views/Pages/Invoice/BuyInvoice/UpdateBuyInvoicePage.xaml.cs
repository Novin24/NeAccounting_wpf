﻿using DomainShared.ViewModels.Pun;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateBuyInvoicePage.xaml
    /// </summary>
    public partial class UpdateBuyInvoicePage : INavigableView<UpdateBuyInvoiceViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public UpdateBuyInvoiceViewModel ViewModel { get; }
        private double _totalEntity;
        private long _price;
        public UpdateBuyInvoicePage(UpdateBuyInvoiceViewModel viewModel, ISnackbarService snackbarService)
        {
            DataContext = this;
            ViewModel = viewModel;
            InitializeComponent();
            _snackbarService = snackbarService;
            txt_CustomerName.Focus();
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
                ViewModel.RemId = null;
                txt_MaterialName.Text = string.Empty;
                txt_UnitName.Text = string.Empty;
                txt_Unit_price.Text = string.Empty;
                txt_total_price.Text = string.Empty;
                txt_UnitDescription.Text = string.Empty;
                txt_MaterialName.Focus();
            }
            dgv_Inv.Items.Refresh();

			ViewModel.LblMatEntityVisibility = Visibility.Visible;
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

			ViewModel.LblMatEntityVisibility = Visibility.Collapsed;
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

        private void txt_CustomerName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = ViewModel.Loding;
        }
    }
}
