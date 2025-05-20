namespace DomainShared.ViewModels.Document
{
    public class RemittanceListViewModel
    {
        /// <summary>
        /// شماره ردیف
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// شناسه حواله خرید
        /// </summary>
        public Guid RremId { get; set; }

        /// <summary>
        /// نام جنس
        /// </summary>
        public string MatName { get; set; }

        /// <summary>
        /// نام واحد
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// قیمت
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// آیا سرویس است
        /// </summary>
        public bool IsService { get; set; }

        /// <summary>
        /// شناسه جنس
        /// </summary>
        public Guid MaterialId { get; set; }

        /// <summary>
        /// مقداری
        /// </summary>
        public double AmountOf { get; set; }

        /// <summary>
        /// جمع کل
        /// </summary>
        public long TotalPrice { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
