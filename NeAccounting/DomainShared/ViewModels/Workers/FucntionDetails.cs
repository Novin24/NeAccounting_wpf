namespace DomainShared.ViewModels.Workers
{
    public struct FucntionDetails
    {
        /// <summary>
        /// شناسه کارکرد
        /// </summary>
        public int Id { get; set; }

        public int persianYear { get; set; }
        public byte persianMonth { get; set; }

        /// <summary>
        /// شناسه کارگر
        /// </summary>
        public Guid WorkerId { get; set; }
    }
}
