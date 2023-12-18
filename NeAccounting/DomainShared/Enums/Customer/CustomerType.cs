using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    /// <summary>
    /// نوع مشتری حقیقی و حقوقی
    /// </summary>
    public enum CustomerType : byte
    {
        /// <summary>
        /// حقوقی
        /// </summary>
        [Display(Name = "حقوقی")]
        Legal,
        /// <summary>
        /// حقیقی
        /// </summary>
        [Display(Name = "حقیقی")]
        True
    }
}