using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
	public class ChequeForPrintDto
	{
		public Guid PayerId { get; set; }
		public Guid ReceverId { get; set; }
		public string PayCusName { get; set; }
		public string PayCusNum { get; set; }
		public string RecCusName { get; set; }
		public string RecCusNum { get; set; }
		public SubmitChequeStatus SubmitStatus { get; set; }
		public ChequeStatus Status { get; set; }
		public string? RecDescripion { get; set; }
		public string? PayDescripion { get; set; }
		public string? DueShamsiDate { get; set; }
		public string? StingShamsiDate { get; set; }
		public string? NationalCode { get; set; }
		public DateTime SubmitDate { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime? TransferDate { get; set; }

		/// <summary>
		/// مبلغ
		/// </summary>
		public long Price { get; set; }

		/// <summary>
		/// سریال چک
		/// </summary>
		public string Cheque_Number { get; set; }

		/// <summary>
		/// سری چک
		/// </summary>
		public string? Cheque_Series { get; set; }
		public string Accunt_Number { get; set; }

		/// <summary>
		/// نام بانک
		/// </summary>
		public string Bank_Name { get; set; }

		/// <summary>
		/// شعبه بانک
		/// </summary>
		public string Bank_Branch { get; set; }
		public string Cheque_Owner { get; set; }
	}
}
