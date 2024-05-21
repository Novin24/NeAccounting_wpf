using Domain.Common;
using DomainShared.Enums;
using DomainShared.Errore;

namespace Domain.NovinEntity.Workers
{
    public class Personel : LocalEntity<Guid>
    {
        #region Navigation
        public ICollection<Salary> Salaries { get; private set; }
        public ICollection<Function> Functions { get; private set; }
        public ICollection<FinancialAid> Aids { get; private set; }
        #endregion

        #region Property
        /// <summary>
        /// نام کامل
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        public string? NationalCode { get; private set; }

        /// <summary>
        /// موبایل
        /// </summary>
        public string Mobile { get; private set; }

        /// <summary>
        /// ادرس
        /// </summary>
        public string? Address { get; private set; }

        /// <summary>
        /// تاریخ شروع به کار
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاریخ اتمام کار
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// شمار پرسنلی
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// شماره حساب
        /// </summary>
        public string AccountNumber { get; private set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// وضعیت شیفت
        /// </summary>
        public Shift ShiftStatus { get; set; }

        /// <summary>
        /// عنوان شغلی
        /// </summary>
        public string JobTitle { get; private set; }

        /// <summary>
        /// دستمزد
        /// </summary>
        public long Salary { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری
        /// </summary>
        public long OverTimeSalary { get; set; }

        /// <summary>
        /// دستمزد هر شیفت
        /// </summary>
        public long ShiftSalary { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری شیفتی
        /// </summary>
        public long ShiftOverTimeSalary { get; set; }

        /// <summary>
        /// حق بیمه
        /// </summary>
        public long InsurancePremium { get; set; }

        /// <summary>
        /// تعداد روز کاری در ماه
        /// </summary>
        public byte DayInMonth { get; set; }

        /// <summary>
        /// وضعیت فعال
        /// </summary>
        public bool IsActive { get; set; }
        #endregion

        #region Constructor
        public Personel()
        {
            Salaries = [];
            Functions = [];
            Aids = [];
        }

        public Personel(
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            int personalId,
            string accountNumber,
            string description,
            string jobTitle,
            DateTime startDate,
            Shift shift,
            long salary,
            long overtimeSalary,
            long shiftSalary,
            long shiftOvertimeSalary,
            long insurancePremium,
            byte dayInMonth)
        {
            SetFullName(fullName);
            SetNationalCode(natinalCode);
            SetMobile(mobile);
            SetAddress(address);
            SetDesc(description);
            SetJobTitele(jobTitle);
            SetAccountNumber(accountNumber);
            DayInMonth = dayInMonth;
            Salary = salary;
            OverTimeSalary = overtimeSalary;
            InsurancePremium = insurancePremium;
            StartDate = startDate;
            PersonnelId = personalId;
            ShiftSalary = shiftSalary;
            ShiftOverTimeSalary = shiftOvertimeSalary;
            Status = Status.InWork;
            ShiftStatus = shift;
            IsActive = true;
        }
        #endregion

        #region Methods
        public Personel SetFullName(string fullName)
        {
            if (fullName.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام پرسنل", "پنجاه"));
            }
            FullName = fullName;
            return this;
        }
        public Personel SetNationalCode(string nationalCode)
        {
            if (!string.IsNullOrEmpty(nationalCode) && nationalCode.Length > 12)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("کدملی", "دوازده"));
            }
            NationalCode = nationalCode;
            return this;
        }
        public Personel SetMobile(string mobile)
        {
            if (mobile.Length > 20)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("موبایل", "بیست"));
            }
            Mobile = mobile;
            return this;
        }
        public Personel SetAddress(string address)
        {
            if (!string.IsNullOrEmpty(address) && address.Length > 150)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("آدرس", "صدوپنجاه"));
            }
            Address = address;
            return this;
        }
        public Personel SetDesc(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "دویست"));
            }
            Description = description;
            return this;
        }
        public Personel SetAccountNumber(string accountNumber)
        {
            if (accountNumber.Length > 26)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("شماره حساب", "بیست و شش"));
            }
            AccountNumber = accountNumber;
            return this;
        }
        public Personel SetJobTitele(string jobTitle)
        {
            if (jobTitle.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("عنوان شغلی", "پنجاه"));
            }
            JobTitle = jobTitle;
            return this;
        }


        public Personel AddSalary(Salary salary)
        {
            Salaries.Add(salary);
            return this;
        }
        public Personel AddAid(FinancialAid aid)
        {
            Aids.Add(aid);
            return this;
        }
        public Personel AddFunction(Function func)
        {
            Functions.Add(func);
            return this;
        }
        #endregion
    }
}
