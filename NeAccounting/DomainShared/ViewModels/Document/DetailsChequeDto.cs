﻿using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
	public class DetailsChequeDto
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
        public DateTime SubmitDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TransferDate{ get; set; }
        public long Price { get; set; }

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
        public string Bank_Name { get; set; }
        public string Bank_Branch { get; set; }
        public string Cheque_Owner { get; set; }
	}
}
