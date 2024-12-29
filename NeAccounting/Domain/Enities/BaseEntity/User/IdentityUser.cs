using Domain.Common;
using DomainShared.Enums.Themes;

namespace Domain.BaseDomain.User
{
    public class IdentityUser : BaseEntity<Guid>
    {
        public IdentityUser()
        {
        }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string PasswordHash { get; set; }
        public string Temp { get; set; }
        public bool IsActive { get; set; }

		/// <summary>
		/// تم برنامه
		/// </summary>
		public Theme CurrentTheme { get; set; }


	}
}
