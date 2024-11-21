using DomainShared.Enums;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

public partial class ImportMaterailViewModel : ObservableObject
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
	/// لیست اجناس
	/// </summary>
	[ObservableProperty]
	private IEnumerable<ImportMaterailListDto> _list;

	public void ReadExcelFile(string filePath)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var list = new List<ImportMaterailListDto>();

		// بارگذاری فایل اکسل
		FileInfo fileInfo = new FileInfo(filePath);
		using (var package = new ExcelPackage(fileInfo))
		{
			var worksheet = package.Workbook.Worksheets[0]; // اولین شیت
			int rowCount = worksheet.Dimension.Rows;

			// فرض می‌کنیم که سطر اول عنوان‌ها هستند
			for (int row = 2; row <= rowCount; row++)
			{
				var materialName = worksheet.Cells[row, 1].Text; 
				var unitName = worksheet.Cells[row, 2].Text; 
				var entity = double.TryParse(worksheet.Cells[row, 3].Text, out double entityValue) ? entityValue : 0; 
				var lastSellPrice = long.TryParse(worksheet.Cells[row, 4].Text, out long lastPriceValue) ? lastPriceValue : 0; 
				var serial = worksheet.Cells[row, 5].Text; 
				var address = worksheet.Cells[row, 6].Text; 

				list.Add(new ImportMaterailListDto
				{
					MaterialName = materialName,
					UnitName = unitName,
					Entity = entity,
					LastSellPrice = lastSellPrice,
					Serial = serial,
					Address = address,
					SEntity = entity.ToString("N0") // فرمت دهی به Entity
				});
			}
		}

		// به روز رسانی لیست در ViewModel
		List = list;
	}
}
