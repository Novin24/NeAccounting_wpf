using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ExporteMaterialViewModel(ISnackbarService snackbarService) : ObservableObject, INavigationAware
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
	private IEnumerable<ExporteMaterialListDto> _list;

	bool IsArchive = false;

	void INavigationAware.OnNavigatedFrom()
	{
	}

	async void INavigationAware.OnNavigatedTo()
	{
		FileName = SetName();
		ExPaht = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
		await InitializeViewModel();
	}

	private static string SetName()
	{
		return Guid.NewGuid().ToString().Replace("-", "") + ".xlsx ";
	}/// <summary>
	 /// پر کردن لیست هنگام ورود به صفحه
	 /// </summary>
	 /// <returns></returns>
	private async Task InitializeViewModel()
	{
		using UnitOfWork db = new();
		List = await db.MaterialManager.GetExporteMaterialList(IsArchive);
	}
	public async Task LoadMaterialList(bool isArchive)
	{
		using UnitOfWork db = new();
		List = await db.MaterialManager.GetExporteMaterialList(isArchive);
	}

	/// <summary>
	/// خروجی اکسل اجناس
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

		var filePath = Path.Combine(ExPaht, FileName);

		using (var package = new ExcelPackage())
		{
			var worksheet = package.Workbook.Worksheets.Add("لیست اجناس");

			// افزودن هدرها
			worksheet.Cells[1, 1].Value = "نام اجناس";
			worksheet.Cells[1, 2].Value = "نام واحد";
			worksheet.Cells[1, 3].Value = "شناسه واحد";
			worksheet.Cells[1, 4].Value = "آخرین قیمت فروش";
			worksheet.Cells[1, 5].Value = "سریال";
			worksheet.Cells[1, 6].Value = "محل نگهداری";

			// افزودن داده‌ها
			for (int i = 0; i < List.Count(); i++)
			{
				var item = List.ElementAt(i);
				worksheet.Cells[i + 2, 1].Value = item.MaterialName;
				worksheet.Cells[i + 2, 2].Value = item.UnitName;
				worksheet.Cells[i + 2, 3].Value = item.UnitNumber;
				worksheet.Cells[i + 2, 4].Value = item.LastSellPrice;
				worksheet.Cells[i + 2, 5].Value = item.Serial;
				worksheet.Cells[i + 2, 6].Value = item.Address;
			}

			// ذخیره فایل
			FileInfo excelFile = new FileInfo(filePath);

			package.SaveAs(excelFile);
			_snackbarService.Show("کاربر گرامی", $"عملیات با موفقیت انجام شد \n فایل {FileName} در مسیر {ExPaht} ذخیره شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(7000));
		}

		FileName = SetName();
	}
}
