using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class CreateRecPage : INavigableView<CreateRecViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreateRecViewModel ViewModel { get; }
        public Guid CusId { get; set; }

        public CreateRecPage(CreateRecViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
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
                btn_submit.Focus();
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
    }
}
