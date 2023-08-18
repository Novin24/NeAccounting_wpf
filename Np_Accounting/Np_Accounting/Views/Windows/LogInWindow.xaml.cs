using Microsoft.Extensions.DependencyInjection;
using Np_Accounting.ViewModels;
using Np_Accounting.Views.Pages;
using System.Windows.Input;

namespace Np_Accounting.Views.Windows
{
    /// <summary>
    /// Interaction logic for LpginPage.xaml
    /// </summary>
    public partial class LogInWindow
    {
        private readonly IServiceProvider _serviceProvider;
        public LogInWindowViewModel ViewModel { get; }

        public LogInWindow(IServiceProvider serviceProvider, LogInWindowViewModel logInViewModel)
        {
            InitializeComponent();
            ViewModel = logInViewModel;
            DataContext = this;
            _serviceProvider = serviceProvider;
            Txt_UserName.Focus();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private async void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.LogIn(Txt_UserName.Text, txt_password.Password))
            {
                Hide();
                if (!System.Windows.Application.Current.Windows.OfType<MainWindow>().Any())
                {
                    var navigationWindow = _serviceProvider.GetRequiredService<MainWindow>();

                    navigationWindow.Loaded += OnNavigationWindowLoaded;
                    navigationWindow!.Show();
                }
            }
            await Task.CompletedTask;
        }

        private void OnNavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not MainWindow navigationWindow)
            {
                return;
            }

            navigationWindow.NavigationView.Navigate(typeof(DashboardPage));
        }
    }
}
