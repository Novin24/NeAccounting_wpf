using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Np_Accounting.Views.Windows
{
    /// <summary>
    /// Interaction logic for LpginPage.xaml
    /// </summary>
    public partial class LogInWindow : INavigationWindow
    {
        public LogInWindow()
        {
            InitializeComponent();
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

        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {

        }


        public Frame GetFrame()
        {
            throw new NotImplementedException();
        }

        public INavigation GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            return true;
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow()
        {
            Show();
        }

        public void CloseWindow()
        {
            throw new NotImplementedException();
        }
    }
}
