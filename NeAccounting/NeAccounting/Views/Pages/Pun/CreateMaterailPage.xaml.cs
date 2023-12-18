using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DefinitionOfPun.xaml
    /// </summary>
    public partial class CreateMaterailPage : INavigableView<CreateMaterailViewModel>
    {
        public CreateMaterailViewModel ViewModel { get; }

        public CreateMaterailPage(CreateMaterailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            MaterialName.Focus();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                ViewModel.UnitId = ((SuggestBoxViewModel<int>)cmb.SelectedItem).Id;
            }

        }
    }
}
