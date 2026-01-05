using Domain.Common;
using Domain.BaseDomain.Menus;
using DomainShared.Enums.Themes;
using DomainShared.Errore;

namespace Domain.BaseDomain.User
{
    public class IdentityUser : BaseEntity<Guid>
    {
        #region navigation
        public ICollection<Menu> Menus { get; set; }
        #endregion


        #region ctor
        public IdentityUser()
        {
            
        }

        public IdentityUser(string userName,
                            string name,
                            string surName,
                            string nationalCode,
                            string mobile,
                            string passwordHash,
                            string temp,
                            ICollection<Menu> userPermission)
        {
            SetUserName(userName);
            SetName(name);
            SetSurName(surName);
            SetNationalCode(nationalCode);
            SetMobile(mobile);
            SetPass(passwordHash);
            SetTemp(temp);
            IsActive = true;
        }
        #endregion

        #region properties
        public string UserName { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string SurName { get; private set; } = default!;
        public string NationalCode { get; private set; } = default!;
        public string Mobile { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string Temp { get; private set; } = default!;
        public DateTime? LastSeen { get; private set; }
        public bool IsActive { get; private set; }

        /// <summary>
        /// تم برنامه
        /// </summary>
        public Theme CurrentTheme { get; set; }

        #endregion

        #region Method
        public IdentityUser SetNationalCode(string nationalCode)
        {
            if (nationalCode.Length > 11)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("کد ملی", "یازده"));
            }
            NationalCode = nationalCode;
            return this;
        }

        public IdentityUser SetSurName(string surName)
        {
            if (surName.Length > 100)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام خانوادگی", "صد"));
            }
            SurName = surName;
            return this;
        }

        public IdentityUser SetName(string name)
        {
            if (name.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام کاربر", "پنجاه"));
            }
            Name = name;
            return this;
        }

        public IdentityUser SetUserName(string userName)
        {
            if (userName.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام کاربری", "پنجاه"));
            }
            UserName = userName;
            return this;
        }

        public IdentityUser SetMobile(string mobile)
        {
            if (mobile.Length > 20)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("موبایل", "بیست"));
            }
            Mobile = mobile;
            return this;
        }

        public IdentityUser SetPass(string password)
        {

            PasswordHash = password;
            return this;
        }

        public IdentityUser SetTemp(string temp)
        {

            Temp = temp;
            return this;
        }

        public IdentityUser SetActive(bool active)
        {

            IsActive = active;
            return this;
        }

        public IdentityUser UpdateLastSeen()
        {

            LastSeen = DateTime.Now;
            return this;
        }
        #endregion


    }
}
