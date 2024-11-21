
using NeAccounting.ViewModels;
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
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ImportMaterailsPage.xaml
    /// </summary>
    public partial class ImportMaterailsPage : INavigableView<ImportMaterailViewModel>
    {
        public ImportMaterailViewModel ViewModel { get; }
        public ImportMaterailsPage(ImportMaterailViewModel viewModel)
        {
            DataContext = this;
            ViewModel = viewModel;
            InitializeComponent();
        }
        private void Btn_Brows_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (dialog.ShowDialog() == true)
            {
                ViewModel.ExPaht = System.IO.Path.GetDirectoryName(dialog.FileName); // آدرس کامل فایل را ذخیره می‌کند
                ViewModel.FileName = System.IO.Path.GetFileName(dialog.FileName); //نام فایل را ذخیره میکند
                ViewModel.ReadExcelFile(dialog.FileName);
            }
        }
    }
}
