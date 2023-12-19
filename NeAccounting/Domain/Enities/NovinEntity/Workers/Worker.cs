using Domain.Common;
using DomainShared.Enums;

namespace Domain.NovinEntity.Workers
{
    public class Worker : LocalEntity
    {
        #region Navigation

        #endregion

        #region Property
        /// <summary>
        /// نام کامل
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// موبایل
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// ادرس
        /// </summary>
        public string Address { get; set; }

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
        public string AccountNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

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
        public string JobTitle { get; set; }

        /// <summary>
        /// دستمزد
        /// </summary>
        public long Salary { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری
        /// </summary>
        public long OverTimeSalary { get; set; }

        /// <summary>
        /// حق بیمه
        /// </summary>
        public long InsurancePremium { get; set; }

        /// <summary>
        /// تعداد روز کاری در ماه
        /// </summary>
        public byte DayInMonth { get; set; }
        #endregion

        #region Constructor
        public Worker()
        {

        }

        public Worker(
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
            long insurancePremium,
            byte dayInMonth)
        {
            FullName = fullName;
            NationalCode = natinalCode;
            Mobile = mobile;
            DayInMonth = dayInMonth;
            Salary = salary;
            OverTimeSalary = overtimeSalary;
            InsurancePremium = insurancePremium;
            Address = address;
            StartDate = startDate;
            PersonnelId = personalId;
            AccountNumber = accountNumber;
            Description = description;
            JobTitle = jobTitle;
            Status = Status.InWork;
            ShiftStatus = shift;
        }
        #endregion
    }
}
