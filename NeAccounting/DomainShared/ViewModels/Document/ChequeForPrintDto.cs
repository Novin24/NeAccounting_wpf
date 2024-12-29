using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
	public class ChequeForPrintDto
	{

		/// <summary>
		/// دریافت کننده چک
		/// </summary>
		public string RecCusName { get; set; }

		/// <summary>
		/// تاریخ شمسی
		/// </summary>
		public string? DueShamsiDate { get; set; }

		/// <summary>
		/// تاریخ شمسی ب حروف
		/// </summary>
		public string? StingShamsiDate { get; set; }

		/// <summary>
		/// کدملی
		/// </summary>
		public string? NationalCode { get; set; }

		/// <summary>
		/// مبلغ
		/// </summary>
		public long Price { get; set; }

		/// <summary>
		/// منفی های پشت مبلغ برای پرینت چک
		/// </summary>
		public string Mines { get; set; }

		/// <summary>
		/// مبلغ به حروف
		/// </summary>
		public String stringPrice { get; set; }

		/// <summary>
		/// سریال چک
		/// </summary>
		public string Cheque_Number { get; set; }

		/// <summary>
		/// سری چک
		/// </summary>
		public string? Cheque_Series { get; set; }

		/// <summary>
		/// شماره صیادی
		/// </summary>
		public string? SiadyNumber { get; set; }

		/// <summary>
		/// شماره شبا
		/// </summary>
		public string? Shaba_Number { get; set; }

		/// <summary>
		/// نام بانک
		/// </summary>
		public string Bank_Name { get; set; }

		/// <summary>
		/// شعبه بانک
		/// </summary>
		public string? Bank_Branch { get; set; }

		/// <summary>
		/// صاحب حساب
		/// </summary>
		public string Cheque_Owner { get; set; }
	}
}
