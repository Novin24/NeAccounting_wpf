using Wpf.Ui.Controls;

namespace Np_Accounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataPage : INavigableView<ViewModels.DataViewModel>
    {
        public ViewModels.DataViewModel ViewModel
        {
            get;
        }

        public DataPage(ViewModels.DataViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void TabControl_Selected(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
