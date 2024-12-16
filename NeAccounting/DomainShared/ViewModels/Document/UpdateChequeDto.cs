using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class UpdateChequeDto
    {
        /// <summary>
        /// docId
        /// </summary>
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CusName { get; set; }
        public string CusNum { get; set; }
        public SubmitChequeStatus SubmitStatus { get; set; }
        public ChequeStatus Status { get; set; }
        public string? Descripion { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime? DueDate { get; set; }
        public long Price { get; set; }
        public string Cheque_Number { get; set; }

		/// <summary>
		/// سری چک
		/// </summary>
		public string? Cheque_Series { get; set; }

		/// <summary>
		/// شماره صیادی
		/// </summary>
		public string? SiadyNumber { get; set; }
		public string? Shaba_Number { get; set; }
        public string Bank_Name { get; set; }
        public string? Bank_Branch { get; set; }
        public string Cheque_Owner { get; set; }
    }
}
