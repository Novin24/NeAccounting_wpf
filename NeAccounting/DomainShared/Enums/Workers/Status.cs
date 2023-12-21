using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    /// <summary>
    /// وضعیت کارگر
    /// </summary>
    public enum Status : byte
    {
        /// <summary>
        /// همه
        /// </summary>
        [Display(Name ="همه")]
        All ,
        /// <summary>
        /// درحال کار
        /// </summary>
        [Display(Name ="مشغول به کار")]
        InWork ,
        /// <summary>
        /// تسویه و اتمام کار
        /// </summary>
        [Display(Name ="تسویه")]
        Settlement,
        /// <summary>
        /// اخراج
        /// </summary>
        [Display(Name ="اخراج")]
        FiredUp
    }
}