using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateExpencePage.xaml
    /// </summary>
    public partial class CreateExpencePage : INavigableView<CreateExpenceViewModel>
    {
        public CreateExpenceViewModel ViewModel { get; }

        public CreateExpencePage(CreateExpenceViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            DateFocus.Focus();
        }
    }
}
