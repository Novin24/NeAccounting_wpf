using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditPunPage.xaml
    /// </summary>
    public partial class UpdateMaterailPage : INavigableView<UpdateMaterailViewModel>
    {
        public UpdateMaterailViewModel ViewModel { get; }


        public UpdateMaterailPage(UpdateMaterailViewModel viewModel)
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
