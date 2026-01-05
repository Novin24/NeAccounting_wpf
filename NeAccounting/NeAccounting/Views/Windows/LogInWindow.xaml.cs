using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NeAccounting.Helpers;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow
    {
        public LoginWindowViewModel ViewModel { get; }

        public LogInWindow(LoginWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            Txt_UserName.Focus();
        }

        private void mainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
            {

                App.Current.Windows[intCounter].Close();
            }
        }


        [RelayCommand]
        private void OnCloseApp()
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async  void Btnlogin_Click(object sender, RoutedEventArgs e)
        {

            Prg_loading.Visibility = Visibility.Visible;
            Btn_LogIn.Visibility = Visibility.Collapsed;
            if (await ViewModel.LogIn(Txt_UserName.Text, txt_password.Password))
            {

                this.DialogResult = true;
                this.Close();
                await Task.CompletedTask;
            }

            Btn_LogIn.Visibility = Visibility.Visible;
            Prg_loading.Visibility = Visibility.Hidden;

        }
    }
}
