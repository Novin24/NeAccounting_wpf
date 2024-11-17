using DomainShared.Constants;
using DomainShared.ViewModels.Customer;
using Infrastructure.UnitOfWork;
using System.Windows.Forms;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ExporteCustomerViewModel : ObservableObject , INavigationAware
{

	/// <summary>
	/// ادرس فایل
	/// </summary>
	[ObservableProperty]
	private string _exPaht;

	/// <summary>
	/// نام فایل
	/// </summary>
	[ObservableProperty]
	private string _fileName;

	/// <summary>
	/// لیست مشتری ها
	/// </summary>
	[ObservableProperty]
	private IEnumerable<ExporteCustomerListDto> _list;

	bool IsArchive = false;

	public void OnNavigatedFrom()
	{

	}

	public async void OnNavigatedTo()
	{
		FileName = SetName();
		await InitializeViewModel();
	}

	/// <summary>
	/// پر کردن لیست هنگام ورود به صفحه
	/// </summary>
	/// <returns></returns>
	private async Task InitializeViewModel()
	{
		using UnitOfWork db = new();
		List = await db.CustomerManager.GetExporteCustomerList(IsArchive);
	}
	public async Task LoadCustomerList(bool isArchive)
	{
		using UnitOfWork db = new();
		List = await db.CustomerManager.GetExporteCustomerList(isArchive);
	}
	public void BrowseFolder()
	{
		using (FolderBrowserDialog dialog = new())
		{
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				ExPaht = dialog.SelectedPath; // آدرس فولدر را به متغیر مربوطه نسبت دهید
			}
		}
	}
	private static string SetName()
	{
		return  Guid.NewGuid().ToString().Replace("-", "") + ".xlsx ";
	}
}
