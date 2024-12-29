using DomainShared.Enums.Themes;

namespace DomainShared.Constants
{
    public static class CurrentUser
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        public static string CurrentUserName;

        /// <summary>
        /// نام کامل کاربر
        /// </summary>
        public static string CurrentFullName;

        /// <summary>
        /// نام کاربر
        /// </summary>
        public static string CurrentName;

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public static Guid CurrentUserId;

		/// <summary>
		/// شناسه کاربر
		/// </summary>
		public static string LogInTime;

		/// <summary>
		/// تم برنامه
		/// </summary>
		public static Theme CurrentTheme;
	}


    public static class EditInvoiceDetails
    {
        public static Guid InvoiceId;
    }
}
