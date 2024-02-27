using DomainShared.ViewModels;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for PayPage.xaml
    /// </summary>
    public partial class PayPage : INavigableView<CreatePayDocViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreatePayDocViewModel ViewModel { get; }
        public Guid CusId { get; set; }

        public PayPage(CreatePayDocViewModel viewModel, ISnackbarService snackbarService)
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

        private async void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.OnSumbit())
            {
                aus_CusName.Text = string.Empty;
                Cmb_PayType.SelectedIndex = 0;
                lbl_cusId.Text = string.Empty;
                lbl_status.Foreground = (Brush)FindResource("TextFillColorPrimaryBrush");
            }
        }

    }
}
