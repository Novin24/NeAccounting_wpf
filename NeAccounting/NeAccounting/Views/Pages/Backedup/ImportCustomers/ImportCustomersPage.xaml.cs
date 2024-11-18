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
    /// Interaction logic for ImportCustomersPage.xaml
    /// </summary>
    public partial class ImportCustomersPage : Page
    {
        public ImportCustomersPage()
        {
            InitializeComponent();
		}
		private void Btn_Brows_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new();

			dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ViewModel.ExPaht = dialog.SelectedPath;
			}
		}
	}
}
