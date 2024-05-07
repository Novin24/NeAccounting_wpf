using Domain.Common;

namespace Domain.BaseDomain.FinancialYears
{
    public class FinancialYear : BaseEntity<Guid>
    {
        #region Navigaton

        #endregion

        #region Properteis
        /// <summary>
        /// عنوان نمایشی
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// guid name
        /// </summary>
        public string DataBaseName { get; set; }
        public string Descripion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        #endregion

        #region Ctor

        public FinancialYear()
        {
        }

        public FinancialYear(string name,
            string dataBaseName,
            string descripion)
        {
            Name = name;
            DataBaseName = dataBaseName;
            Descripion = descripion;
            StartDate = DateTime.Now;
            IsActive = true;
            IsCurrent = true;
        }
        #endregion
    }
}
