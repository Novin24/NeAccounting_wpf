using NeAccounting.ViewModels;

namespace NeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for WatingWindow.xaml
    /// </summary>
    public partial class WatingWindow 
    {
        public WatingWindowViewModel ViewModel { get; }

        public WatingWindow(WatingWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private async void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not WatingWindow ww)
            {
                return;
            }
            Btn_start.Visibility = Visibility.Collapsed;
            btn_End.Visibility = Visibility.Visible;
            await ww.ViewModel.ChangeYearCommand.ExecuteAsync(null);
            btn_End.Visibility = Visibility.Collapsed;
            btn_close.Visibility = Visibility.Visible;
        }

        private void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
