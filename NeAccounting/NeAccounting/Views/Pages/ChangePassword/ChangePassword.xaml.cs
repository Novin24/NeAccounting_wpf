using PasswordStrengthFinder;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
            PasswordValidator passwordValidator;
        public ChangePassword()
        {
             passwordValidator = new PasswordValidator();
            InitializeComponent();
            
        }

        private void btnSetPassword(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == string.Empty) return;
            
            int Value = 0;
            if (passwordValidator.IsStrong(txtPassword.Text, out Value))
            {
                Power.Value = Value;
            }

        }
    }
}
