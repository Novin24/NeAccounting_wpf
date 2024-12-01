using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DebtorsListPage.xaml
    /// </summary>
    public partial class DebtorsListPage : INavigableView<DebtorsViewModel>
    {
        public DebtorsViewModel ViewModel { get; }

        public DebtorsListPage(DebtorsViewModel viewModel)
        {
			ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Dtp_Start.nm_box.Focus();
		}
	}
}
