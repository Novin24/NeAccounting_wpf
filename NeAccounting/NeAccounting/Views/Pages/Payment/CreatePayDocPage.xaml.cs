﻿using DomainShared.ViewModels;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for PayPage.xaml
    /// </summary>
    public partial class CreatePayDocPage : INavigableView<CreatePayDocViewModel>
    {
        public CreatePayDocViewModel ViewModel { get; }

        public CreatePayDocPage(CreatePayDocViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private async void AutoSuggestBoxSuggestions_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
            await ViewModel.OnSelectCus(user.Id);
            if (ViewModel.Status == "بدهکار")
            {
                lbl_status.Foreground = new SolidColorBrush(Colors.IndianRed);
            }

            else if (ViewModel.Status == "طلبکار")
            {
                lbl_status.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }

            else
            {
                lbl_status.Foreground = (Brush)FindResource("TextFillColorPrimaryBrush");
            }
        }
        [RelayCommand]
        private async Task OnSubmit()
        {
            btn_submit.Focus();
            if (await ViewModel.OnSumbit())
            {
                aus_CusName.Text = string.Empty;
                Cmb_PayType.SelectedIndex = 0;
                lbl_cusId.Text = string.Empty;
                lbl_status.Foreground = (Brush)FindResource("TextFillColorPrimaryBrush");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            aus_CusName.Focus();
        }

        private void txt_CustomerName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = ViewModel.Loding;
        }
    }
}
