using DomainShared.ViewModels;
using NeAccounting.ViewModels.Pages;
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
        public int materailId { get; set; }


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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cmb)
            {
                if (cmb.IsChecked.Value)
                {
                    EntityNumbox.Value = 0;
                    EntityNumbox.IsEnabled = false;
                    return;
                }
                EntityNumbox.IsEnabled = true;
            }
        }
    }
}
