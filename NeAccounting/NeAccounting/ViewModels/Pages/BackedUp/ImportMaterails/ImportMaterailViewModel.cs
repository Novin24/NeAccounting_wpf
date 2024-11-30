using Domain.NovinEntity.Materials;
using DomainShared.Enums;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ImportMaterailViewModel(ISnackbarService snackbarService) : ObservableObject
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
	/// ادرس فایل نمونه اکسل
	/// </summary>
	[ObservableProperty]
	private string _exPahtExport;

	/// <summary>
	/// نام فایل نمونه اکسل
	/// </summary>
	[ObservableProperty]
	private string _fileNameExport;

	/// <summary>
	/// لیست اجناس
	/// </summary>
	[ObservableProperty]
	private IEnumerable<ImportMaterailListDto> _list;

	private static string SetName()
	{
		return Guid.NewGuid().ToString().Replace("-", "") + ".xlsx ";
	}

	public async void ReadExcelFile(string filePath)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var list = new List<ImportMaterailListDto>();

		// بارگذاری فایل اکسل
		FileInfo fileInfo = new FileInfo(filePath);
		using (var package = new ExcelPackage(fileInfo))
		{
			var worksheet = package.Workbook.Worksheets[0]; // اولین شیت
			int rowCount = worksheet.Dimension.Rows;
			using (UnitOfWork db = new())
			{
				// فرض می‌کنیم که سطر اول عنوان‌ها هستند
				for (int row = 2; row <= rowCount; row++)
				{
					var materialName = worksheet.Cells[row, 1].Text;

					var unitNumberText = worksheet.Cells[row, 2].Text;
					int unitNumber = Convert.ToInt32(unitNumberText);

					var lastSellPrice = long.TryParse(worksheet.Cells[row, 3].Text, out long lastPriceValue) ? lastPriceValue : 0;
					var serial = worksheet.Cells[row, 4].Text;
					var address = worksheet.Cells[row, 5].Text;
					var (error, unitId) = await db.UnitManager.GetUnitIdByUnitNumber(unitNumber);
					list.Add(new ImportMaterailListDto
					{
						MaterialName = materialName,
						UnitNumber = unitNumber,
						UnitId = unitId,
						LastSellPrice = lastSellPrice,
						Serial = serial,
						Address = address,
					});
				}
			}
		}

		// به روز رسانی لیست در ViewModel
		List = list;
	}

	[RelayCommand]
	private async Task OnImportMaterials()
	{
		if (List == null || !List.Any())
		{
			_snackbarService.Show("خطا", "هیچ جنسی برای وارد کردن وجود ندارد.", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
			return;
		}
		using (UnitOfWork db = new())
		{
			foreach (var item in List)
			{
				var (error, isSuccess, Show) = await db.MaterialManager.CreateMaterial(
					item.MaterialName,
					item.UnitId,
					false,
					item.LastSellPrice,
					item.Serial,
					item.Address,
					false
				);

				if (!isSuccess & Show)
				{
					_snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
					return;
				}
			}
		}

		_snackbarService.Show("موفقیت", "تمامی اجناس با موفقیت وارد شدند.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
	}

	/// <summary>
	/// نمونه اکسل
	/// </summary>
	/// <returns></returns>
	[RelayCommand]
	public async Task OnExportToExcel()

	{
		FileNameExport = SetName();
		ExPahtExport = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		// ایجاد داده‌های نمونه
		var sampleData = new List<ExporteMaterialListDto>
	{
		new ExporteMaterialListDto { MaterialName = "سیب", UnitNumber = 6, LastSellPrice = 3000, Serial = "S001", Address = "قفسه 1" },
		new ExporteMaterialListDto { MaterialName = "نان", UnitNumber = 5, LastSellPrice = 1500, Serial = "S002", Address = "قفسه 2" },
		new ExporteMaterialListDto { MaterialName = "شیر", UnitNumber = 7, LastSellPrice = 4000, Serial = "S003", Address = "قفسه 3" }
	};

		var filePath = Path.Combine(ExPahtExport, FileNameExport);

		using (var package = new ExcelPackage())
		{
			var worksheet = package.Workbook.Worksheets.Add("لیست اجناس");

			// افزودن هدرها
			worksheet.Cells[1, 1].Value = "نام کالا";
			worksheet.Cells[1, 2].Value = "شناسه واحد";
			worksheet.Cells[1, 3].Value = "آخرین قیمت فروش";
			worksheet.Cells[1, 4].Value = "سریال";
			worksheet.Cells[1, 5].Value = "محل نگهداری";

			// افزودن داده‌ها
			for (int i = 0; i < sampleData.Count; i++)
			{
				var item = sampleData[i];
				worksheet.Cells[i + 2, 1].Value = item.MaterialName;
				worksheet.Cells[i + 2, 2].Value = item.UnitNumber;
				worksheet.Cells[i + 2, 3].Value = item.LastSellPrice;
				worksheet.Cells[i + 2, 4].Value = item.Serial;
				worksheet.Cells[i + 2, 5].Value = item.Address;
			}

			// ذخیره فایل
			FileInfo excelFile = new FileInfo(filePath);
			package.SaveAs(excelFile);
			_snackbarService.Show("کاربر گرامی", $"عملیات با موفقیت انجام شد \n فایل {FileNameExport} در مسیر {ExPahtExport} ذخیره شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(9000));
		}

		FileNameExport = SetName();
	}

}
