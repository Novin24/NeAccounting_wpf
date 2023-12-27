using DomainShared.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Salary.xaml
    /// </summary>
    public partial class CreateSalaryPage : INavigableView<CreateSalaryViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreateSalaryViewModel ViewModel { get; }
        public CreateSalaryPage(CreateSalaryViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
        }

        private async void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

            ViewModel.WorkerId = ((SuggestBoxViewModel<int>)args.SelectedItem).Id;
            await ViewModel.OnSelect();
            if (ViewModel.Worker.Shift == DomainShared.Enums.Shift.ByHour)
            {

            }

        }

        private void SetTotalPlusPrice()
        {
            txt_totalPlus.Value = txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value;
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
