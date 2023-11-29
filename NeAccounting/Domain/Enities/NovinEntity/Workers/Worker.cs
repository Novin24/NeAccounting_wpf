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
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// تاریخ اتمام کار
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// شمار پرسنلی
        /// </summary>
        public string PersonnelId { get; set; }

        /// <summary>
        /// شماره حساب
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

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
            DateOnly startDate,
            string personalId,
            string accountNumber,
            string description,
            string jobTitle,
            Shift shift)
        {
            FullName = fullName;
            NationalCode = natinalCode;
            Mobile = mobile;
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
