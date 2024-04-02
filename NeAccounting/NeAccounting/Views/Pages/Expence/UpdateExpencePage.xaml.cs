using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateExpencePage.xaml
    /// </summary>
    public partial class UpdateExpencePage : INavigableView<UpdateExpenceViewModel>
    {

        public UpdateExpenceViewModel ViewModel { get; }

        public UpdateExpencePage(UpdateExpenceViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_Titele.Focus();
        }
    }
}
