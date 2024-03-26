// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using NeAccounting.ViewModels;
using System.Windows.Input;
using Wpf.Ui;
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void Btnlogin_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.LogIn(Txt_UserName.Text, txt_password.Password))
            {
                mainWin.MinWidth = 1200;
                LoginGrid.Visibility = Visibility.Collapsed;
                LoginGrid.IsEnabled = false;

                this.WindowStyle = WindowStyle.SingleBorderWindow;
                mainGrid.Visibility = Visibility.Visible;
                mainGrid.IsEnabled = true;
                mainWin.WindowState = WindowState.Maximized;
            }
            await Task.CompletedTask;
        }
    }
}
