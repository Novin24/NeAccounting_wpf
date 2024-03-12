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
        SellInv = 3,
        /// <summary>
        /// فاکتور خرید
        /// </summary>
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
        Other

    }
}