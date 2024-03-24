using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    public enum ChequeStatus : byte
    {
        /// <summary>
        /// همه چک ها
        /// </summary>
        [Display(Name = "همه چک ها")]
        AllCheques = 1,
        /// <summary>
        /// نقد شده
        /// </summary>
        [Display(Name = "نقد شده")]
        Cashed,
        /// <summary>
        /// برگشت شده
        /// </summary>
        [Display(Name = "برگشت شده")]
        Rejected,
        /// <summary>
        /// پرداختی
        /// </summary>
        [Display(Name = "پرداختی")]
        Payed,
        /// <summary>
        /// ضمانتی
        /// </summary>
        [Display(Name = "ضمانتی")]
        Guarantee,
        /// <summary>
        /// واگذار شده
        /// </summary>
        [Display(Name = "واگذار شده")]
        Transferred,
        /// <summary>
        /// داخل صندوق
        /// </summary>
        [Display(Name = "داخل صندوق")]
        InBox
    }
}