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
        public int WorkerId { get; set; }

        public int PersianYear { get; set; }
        public byte PersianMonth { get; set; }
    }
}
