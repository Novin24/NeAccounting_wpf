using DomainShared.Enums;
using DomainShared.Extension;
using System.Globalization;

namespace DomainShared.ViewModels.Expense
{
    public class ExpenselistDto
    {
        public Guid Id { get; set; }
        public long Price { get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; set; }
        public string ShamsiDate
        {
            get
            {
                PersianCalendar xe = new();
                return Date.ToShamsiDate(xe);
            }
        }
        public PaymentType Type { get; set; }
        public string Expensetype { get; set; }
        public string Description { get; set; }
    }
}
