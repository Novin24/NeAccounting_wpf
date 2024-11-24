using DomainShared.Constants;
using DomainShared.ViewModels.Customer;
using Infrastructure.UnitOfWork;
using System.Windows.Forms;
using Wpf.Ui.Controls;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Windows;
using DomainShared.Enums;
using Wpf.Ui;
using static Stimulsoft.Report.StiOptions;

namespace NeAccounting.ViewModels;

public partial class ExporteCustomerViewModel(ISnackbarService snackbarService) : ObservableObject, INavigationAware
{

	private readonly ISnackbarService _snackbarService = snackbarService;
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
		ExPaht = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
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
				ExPaht = dialog.SelectedPath; 
			}
		}
	}
	private static string SetName()
	{
		return Guid.NewGuid().ToString().Replace("-", "") + ".xlsx ";
	}

	/// <summary>
	/// خروجی اکسل کاربران
	/// </summary>
	/// <returns></returns>
	[RelayCommand]
	public async Task OnExportToExcel()
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		if (List == null || !List.Any())
		{
			return;
		}

		var filePath = Path.Combine(ExPaht, $"{FileName}");

		using (ExcelPackage package = new ExcelPackage())
		{
			var worksheet = package.Workbook.Worksheets.Add("Customers");
			// اضافه کردن توضیحات
			worksheet.Cells[1, 1].Value = "نکته : 0 به منزله ی ( خیر ) و 1 به منزله ی ( بله ) است";
			worksheet.Cells[1, 1, 1, 7].Merge = true; // ادغام سلول‌ها برای توضیحات
			worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // تنظیم تراز وسط
																												   // اضافه کردن هدرها
			worksheet.Cells[2, 1].Value = "نام";
			worksheet.Cells[2, 2].Value = "کد ملی";
			worksheet.Cells[2, 3].Value = "نوع مشتری(0 به مزله ی حقوقی و 1 به منزله ی حقیقی است)";
			worksheet.Cells[2, 4].Value = "موبایل";
			worksheet.Cells[2, 5].Value = "خریدار";
			worksheet.Cells[2, 6].Value = "فروشنده";
			worksheet.Cells[2, 7].Value = "آدرس";

			// اضافه کردن داده‌ها
			int row = 3;
			foreach (var customer in List)
			{
				worksheet.Cells[row, 1].Value = customer.Name;
				worksheet.Cells[row, 2].Value = customer.NationalCode;
				worksheet.Cells[row, 3].Value = customer.CusType == CustomerType.Legal ? "0" : "1";
				worksheet.Cells[row, 4].Value = customer.Mobile;
				worksheet.Cells[row, 5].Value = customer.Buyer ? "1" : "0";
				worksheet.Cells[row, 6].Value = customer.Seller ? "1" : "0";
				worksheet.Cells[row, 7].Value = customer.Address;
				row++;
			}

			// تنظیم عرض ستون‌ها
			for (int i = 1; i <= 7; i++)
			{
				worksheet.Column(i).AutoFit();
			}

			// ذخیره‌سازی فایل
			var fileInfo = new FileInfo(filePath);
			package.SaveAs(fileInfo);
			_snackbarService.Show("کاربر گرامی", $"عملیات با موفقیت انجام شد \n فایل {FileName} در مسیر {ExPaht} ذخیره شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(7000));
		}

		FileName = SetName();
	}

}
