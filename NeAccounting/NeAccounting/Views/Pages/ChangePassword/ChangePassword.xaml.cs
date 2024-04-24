using NeAccounting.Helpers.Extention;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : INavigableView<ChangePassViewModel>
    {
        private readonly PasswordValidator passwordValidator;
        public ChangePassViewModel ViewModel { get; }
        public ChangePassword(ChangePassViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            passwordValidator = new();
            txt_CurPass.Focus();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                Power.Visibility = Visibility.Hidden;
                lbl_PasswordOutput.Visibility = Visibility.Hidden;
                return;
            }
            Power.Visibility = Visibility.Visible;
            lbl_PasswordOutput.Visibility = Visibility.Visible;
            var (t, s) = passwordValidator.IsStrong(txtPassword.Password, out string message);
            lbl_PasswordOutput.Text = message;
            Power.Value = s;
            Power.Foreground = s switch
            {
                40 => new SolidColorBrush(Colors.OrangeRed),
                60 => new SolidColorBrush(Color.FromArgb(0xFF, 207, 176, 0)),
                80 => new SolidColorBrush(Color.FromArgb(0xFF, 107, 191, 11)),
                100 => new SolidColorBrush(Color.FromArgb(0xFF, 24, 181, 52)),
                _ => new SolidColorBrush(Color.FromArgb(0xFF, 198, 29, 29)),
            };
        }

        [RelayCommand]
        private async Task OnChangePass()
        {
            Btn_submit.Focus();
            await ViewModel.ChangePassCommand.ExecuteAsync(null);
        }
    }
}
