using Domain.Common;

namespace Domain.BaseDomain.FinancialYears
{
    public class FinancialYear : BaseEntity<Guid>
    {
        public FinancialYear()
        {
        }

        public string Name { get; set; }
        public string DataBaseName { get; set; }
        public string Descripion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
