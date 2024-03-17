using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    public enum SubmitChequeStatus : byte
    {
        /// <summary>
        /// ثبت شده
        /// </summary>
        [Display(Name = "ثبت شده")]
        Register = 1,
        /// <summary>
        /// ثبت نشده
        /// </summary>
        [Display(Name = "ثبت نشده")]
        NotRegister,
        /// <summary>
        /// بدون نیاز به ثبت
        /// </summary>
        [Display(Name = "بدون نیاز به ثبت")]
        NoNeedRegister,
    }
}