using DomainShared.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Salary.xaml
    /// </summary>
    public partial class CreateSalaryPage : INavigableView<CreateSalaryViewModel>
    {
        public CreateSalaryViewModel ViewModel { get; }
        public CreateSalaryPage(CreateSalaryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

            ViewModel.WorkerId = ((SuggestBoxViewModel<int>)args.SelectedItem).Id;
        }

        private void SetTotalPlusPrice()
        {
            txt_totalPlus.Value += input;
        }

        private void NumberBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((NumberBox)sender).Value;
            if (input != null && input != 0)
            {
                SetTotalPlusPrice();
            }
        }
    }
}
