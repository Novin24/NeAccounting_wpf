
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateUserPage.xaml
    /// </summary>
    public partial class CreateUserPage : INavigableView<CreateUserViewModel>
    {
        public CreateUserViewModel ViewModel { get; }

        public CreateUserPage(CreateUserViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_UserName.Focus();
        }


        [RelayCommand]
        private async Task OnCreateUsre()
        {
            Btn_submit.Focus();
            await ViewModel.CreateUserCommand.ExecuteAsync(null);
        }
    }
}
