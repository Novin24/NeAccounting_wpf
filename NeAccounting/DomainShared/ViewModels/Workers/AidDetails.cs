namespace DomainShared.ViewModels.Workers
{
    public struct AidDetails
    {
        /// <summary>
        /// شناسه مساعده
        /// </summary>
        public int Id { get; set; }


        public byte PersianMonth { get; set; }

        public int PersianYear { get; set; }
        /// <summary>
        /// شناسه کارگر
        /// </summary>
        public Guid WorkerId { get; set; }
    }
}
