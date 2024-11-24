namespace DomainShared.ViewModels.unit
{
    public class UnitListDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// نام واحد
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// آیا فعال است
        /// </summary>
        public bool IsActive { get; set; }

		/// <summary>
		/// شماره واحد
		/// </summary>
		public int IdNumber { get; set; } = 86210;
	}
}
