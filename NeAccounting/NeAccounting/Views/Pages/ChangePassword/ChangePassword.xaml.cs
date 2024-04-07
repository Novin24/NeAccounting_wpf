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
using System.Windows.Forms;
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


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
            if (txtPassword.Text == string.Empty) return;

            string message = string.Empty;
            var (t, s) = passwordValidator.IsStrong(txtPassword.Text, out message);
            lblPasswordOutput.Text = message;
            Power.Value = s;
            switch (s)
            {
                case 40:
                    Power.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    break;
                case 60:
                    Power.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 207, 176, 0));
                    break;
                case 80:
                    Power.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 107, 191, 11));
                    break;
                case 100:
                    Power.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 24, 181, 52));
                    break;
                default:
                    Power.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 198, 29, 29));
                    break;
            }
        }
    }
}
