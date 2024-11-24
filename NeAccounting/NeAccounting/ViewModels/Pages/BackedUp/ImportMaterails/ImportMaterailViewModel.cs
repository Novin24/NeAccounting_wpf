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
	/// لیست اجناس
	/// </summary>
	[ObservableProperty]
	private IEnumerable<ImportMaterailListDto> _list;

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
					var unitName = worksheet.Cells[row, 2].Text;

					var lastSellPrice = long.TryParse(worksheet.Cells[row, 3].Text, out long lastPriceValue) ? lastPriceValue : 0;
					var serial = worksheet.Cells[row, 4].Text;
					var address = worksheet.Cells[row, 5].Text;
					var(error, unitId) = await db.UnitManager.GetUnitIdByName(unitName);
					list.Add(new ImportMaterailListDto
					{
						MaterialName = materialName,
						UnitName = unitName,
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
				var (error, isSuccess) = await db.MaterialManager.CreateMaterial(
					item.MaterialName,
					item.UnitId,
					false, 
					item.LastSellPrice,
					item.Serial,
					item.Address,
					false 
				);

				if (!isSuccess)
				{
					_snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
					return;
				}
			}
		}

		// به‌روزرسانی لیست یا انجام کارهای دیگر پس از ثبت
	}
}
