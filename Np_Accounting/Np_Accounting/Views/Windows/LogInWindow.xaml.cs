using Np_Accounting.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Gallery.Services.Contracts;
using Wpf.Ui.Mvvm.Contracts;

namespace Np_Accounting.Views.Windows
{
    /// <summary>
    /// Interaction logic for LpginPage.xaml
    /// </summary>
    public partial class LogInWindow : IWindow
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigationWindow _navigationWindow;
        public LogInViewModel _logInViewModel { get; }

        public LogInWindow(IServiceProvider serviceProvider, LogInViewModel logInViewModel)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _logInViewModel = logInViewModel;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private async void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            var t = _logInViewModel.LogIn(Txt_UserName.Text, txt_password.Password);
            await Task.CompletedTask;
            Hide();

            if (!System.Windows.Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _navigationWindow = (_serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow)!;
                _navigationWindow!.ShowWindow();

                _navigationWindow.Navigate(typeof(Pages.DashboardPage));
            }

            await Task.CompletedTask;
        }


    }
}
