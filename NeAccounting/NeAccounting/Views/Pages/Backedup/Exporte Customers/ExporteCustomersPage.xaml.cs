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
	/// Interaction logic for ExporteCustomersPage.xaml
	/// </summary>
	public partial class ExporteCustomersPage : INavigableView<ExporteCustomerViewModel>
	{
		public ExporteCustomerViewModel ViewModel { get; }
		public ExporteCustomersPage(ExporteCustomerViewModel viewModel)
		{
			DataContext = this;
			ViewModel = viewModel;
			InitializeComponent();
			txt_name.Focus();
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
		private async void CheckBox_IsArchive_Checked(object sender, RoutedEventArgs e)
		{
			ViewModel.LoadCustomerList(true);
		}

		private async void CheckBox_IsArchive_Unchecked(object sender, RoutedEventArgs e)
		{
			ViewModel.LoadCustomerList(false);
		}
		private async void Btn_Export_Click(object sender, RoutedEventArgs e)
		{
			await ViewModel.ExportToExcel();
		}

	}
}
