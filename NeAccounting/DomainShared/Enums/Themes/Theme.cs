using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums.Themes
{
    public enum Theme : byte
    {
        /// <summary>
        /// تیره
        /// </summary>
        [Display(Name = "Dark")]
        Dark,
        /// <summary>
        /// روشن
        /// </summary>
        [Display(Name = "Light")]
        Light
    }
}