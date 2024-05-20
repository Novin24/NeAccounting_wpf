// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using NeAccounting.Helpers;
using NeAccounting.ViewModels;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;
//using Wpf.Ui.Controls;

namespace NeAccounting.Windows
{
    public partial class MainWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {

            ViewModel = viewModel;
            DataContext = this;

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);
            Txt_UserName.Focus();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void OnCloseApp()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void OnNavigateDashboard()
        {
            Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

            if (pageType == null)
            {
                return;
            }
            NavigationView.Navigate(pageType);
        }

        private async void Btnlogin_Click(object sender, RoutedEventArgs e)
        {
            Btn_LogIn.Visibility = Visibility.Collapsed;
            Prg_loading.Visibility = Visibility.Visible;
            if (await ViewModel.LogIn(Txt_UserName.Text, txt_password.Password))
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                LoginGrid.IsEnabled = false;

                this.WindowStyle = WindowStyle.SingleBorderWindow;
                mainGrid.Visibility = Visibility.Visible;
                mainGrid.IsEnabled = true;
                mainWin.WindowState = WindowState.Maximized;

                Type? pageType = NameToPageTypeConverter.Convert("Dashboard");

                if (pageType == null)
                {
                    return;
                }
                NavigationView.Navigate(pageType);
                key_esc.Command = NavigateDashboardCommand;
                await Task.CompletedTask;
            }

            Btn_LogIn.Visibility = Visibility.Visible;
            Prg_loading.Visibility = Visibility.Hidden;
        }

        private void mainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
            {

                App.Current.Windows[intCounter].Close();
            }
        }
    }
}
