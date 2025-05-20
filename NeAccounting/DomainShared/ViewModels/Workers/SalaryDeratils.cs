namespace DomainShared.ViewModels.Workers
{
    public struct SalaryDetails
    {
        /// <summary>
        /// شناسه فیش حقوقی
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه کارگر
        /// </summary>
        public Guid WorkerId { get; set; }

        /// <summary>
        /// سال شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public byte PersianMonth { get; set; }
    }
}
