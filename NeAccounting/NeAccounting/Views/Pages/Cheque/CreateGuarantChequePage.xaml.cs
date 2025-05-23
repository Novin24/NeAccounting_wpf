﻿using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateGuarantChequePage.xaml
    /// </summary>
    public partial class CreateGuarantChequePage : INavigableView<CreateGuarantChequeViewModel>
    {
        public CreateGuarantChequeViewModel ViewModel { get; }
        public CreateGuarantChequePage(CreateGuarantChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CreateGuarantChequePage pg) { return; }
            if (pg.ViewModel.CusId.HasValue)
            {
                txt_Name.Text = pg.ViewModel.CusName;
                lbl_cusId.Text = pg.ViewModel.CusNum;
                txt_Name.IsEnabled = false;
            }
            else
            {
                txt_Name.Focus();
            }
            Cmb_Status.ItemsSource = SubmitChequeStatus.Register.ToEnumDictionary();
        }

        private void Txt_Name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }

        [RelayCommand]
        private async Task OnSubmit()
        {
            Btn_submit.Focus();
            await ViewModel.SubmitCommand.ExecuteAsync(null);
        }
        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();

        private void txt_CustomerName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = ViewModel.Loding;
        }
    }
}
