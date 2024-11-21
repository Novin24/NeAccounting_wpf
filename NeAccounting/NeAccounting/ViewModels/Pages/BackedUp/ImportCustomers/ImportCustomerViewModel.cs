using DomainShared.Enums;
using DomainShared.ViewModels.Customer;
using Infrastructure.UnitOfWork;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels;

	public partial class ImportCustomerViewModel(ISnackbarService snackbarService) : ObservableObject
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
	private IEnumerable<ImportCustomerListDto> _list;


	public void ReadExcelFile(string filePath)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var customers = new List<ImportCustomerListDto>();

		using (var package = new ExcelPackage(new FileInfo(filePath)))
		{
			var worksheet = package.Workbook.Worksheets[0]; // اولین شیت را انتخاب می‌کند

			// شروع از ردیف 2 (ردیف 1 شامل عنوان ستون‌هاست)
			for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
			{
				var customer = new ImportCustomerListDto
				{
					Name = worksheet.Cells[row, 1].Text,
					NationalCode = worksheet.Cells[row, 2].Text,
					CusType = worksheet.Cells[row, 3].Text == "0" ? CustomerType.Legal : CustomerType.True, // تغییرات برای CusType
					CusTypeName = worksheet.Cells[row, 3].Text == "0" ? "حقوقی" : "حقیقی", // تغییرات برای CusTypeName
					Mobile = worksheet.Cells[row, 4].Text,
					Buyer = worksheet.Cells[row, 5].Text == "1", // تغییرات برای Buyer
					Seller = worksheet.Cells[row, 6].Text == "1", // تغییرات برای Seller
					Address = worksheet.Cells[row, 7].Text
				};
				customers.Add(customer);
			}
		}

		List = customers; // لیست مشتری‌ها را به روز می‌کند
	}

	[RelayCommand]
	private async Task OnImportCustomer()
	{
		if (List == null || !List.Any())
		{
			_snackbarService.Show("خطا", "هیچ کاربری برای وارد کردن وجود ندارد.", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
			return;
		}

		using (UnitOfWork db = new())
		{
			foreach (var customer in List)
			{
				var (error, isSuccess , Show) = await db.CustomerManager.CreateCustomer(
					customer.Name,
					customer.Mobile,
					0, // اعتبار نقدی یا سفته را بر اساس نیاز خود تنظیم کنید
					0,
					customer.NationalCode,
					customer.Address,
					customer.CusType,
					false,
					false,
					customer.Buyer,
					customer.Seller);

				if (!isSuccess & Show)
				{
					_snackbarService.Show("خطا", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
					return;
				}
			}
		}

		_snackbarService.Show("موفقیت", "تمامی کاربران با موفقیت وارد شدند.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
	}
}
