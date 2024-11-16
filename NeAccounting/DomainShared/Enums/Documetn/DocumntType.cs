using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    public enum DocumntType : byte
    {
        /// <summary>
        /// اسناد پرداختی
        /// </summary>
        PayDoc = 1,
        /// <summary>
        /// اسناد دریافتی
        /// </summary>
        RecDoc = 2,
        /// <summary>
        /// فاکتور فروش
        /// </summary>
        [Display(Name ="فاکتور فروش")]
        SellInv = 3,
        /// <summary>
        /// فاکتور خرید
        /// </summary>
        [Display(Name ="فاکتور خرید")]
        BuyInv = 4,
        /// <summary>
        /// پورسانت دریافتی
        /// </summary>
        RecCom,
        /// <summary>
        /// پورسانت پرداختی
        /// </summary>
        PayCom,
        /// <summary>
        /// تخفیف گرفته شده
        /// </summary>
        PayDiscount,
        /// <summary>
        /// تخفیف داده شده
        /// </summary>
        RecDiscount,
        /// <summary>
        /// چک
        /// </summary>
        Cheque,
        /// <summary>
        /// چک ضمانتی
        /// </summary>
        GarantyCheque,
        /// <summary>
        /// برگشت از فروش
        /// </summary>
        ReturnFromSell,
        /// <summary>
        /// برگشت از خرید
        /// </summary>
        ReturnFromBuy,

        Other = 59,

        LeftOver = 60

    }
}