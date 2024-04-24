using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CheckDetailsPage.xaml
    /// </summary>
    public partial class CheckDetailsPage : INavigableView<DetailsChequeViewModel>
    {

        public DetailsChequeViewModel ViewModel { get; }

        public CheckDetailsPage(DetailsChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CheckDetailsPage c && c.ViewModel.Status != DomainShared.Enums.ChequeStatus.Transferred)
            {
                Txt_RecDescription.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(Txt_PayDescription, 2);
                txt_TransferDate.Visibility = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private void OnBackClick()
        {
            Btn_submit.Focus();
             ViewModel.BackClickCommand.Execute(null);
        }
    }
}
