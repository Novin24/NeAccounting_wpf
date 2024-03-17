using Domain.NovinEntity.Expense;
using Domain.NovinEntity.Materials;
using DomainShared.Enums;
using DomainShared.Extension;
using DomainShared.Utilities;
using DomainShared.ViewModels.Expense;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies.Expens;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    public class ExpenseManager : Repository<Expense>, IExpenseManager
    {
        public ExpenseManager(NovinDbContext context) : base(context) { }

        public async Task<(string error, bool isSuccess)> CreateExpense(DateTime submitDate, string expensetype, long amount, PaymentType payType, string receiver, string description)
        {
            if (await TableNoTracking.AnyAsync(t => t.Expensetype == expensetype))
                return new("کاربر گرامی این هزینه از قبل تعریف شده می‌باشد!!!", false);

            try
            {

                var t = await Entities.AddAsync(new Expense(submitDate,
                                expensetype,
                                amount,
                                payType,
                                receiver,
                                description));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);

        }
        public async Task<(string error, bool isSuccess)> UpdateExpense(Guid expenseID, DateTime submitDate, string expensetype, long amount, PaymentType payType, string receiver, string description)
        {
            try
            {
                var x = await Entities.FindAsync(expenseID);

                if (x == null)
                    return new(" مورد  مد نظر یافت نشد !!!", false);

                x.SubmitDate = submitDate;
                x.Expensetype = expensetype;
                x.Amount = amount;
                x.PayType = payType;
                x.Receiver = receiver;
                x.Description = description;

                Update(x, false);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        public async Task<List<ExpenselistDto>> GetExpenselist(DateTime? startDate, DateTime? endDate)
        {
            PersianCalendar xe = new();
            return await TableNoTracking
                .Where(x => !startDate.HasValue || x.SubmitDate >= startDate)
                .Where(x => !endDate.HasValue || x.SubmitDate < endDate)
                .Select(e => new ExpenselistDto()
                {
                    Id = e.Id,
                    Receiver = e.Receiver,
                    Date = e.SubmitDate.ToShamsiDate(xe),
                    type = e.PayType.ToDisplay(DisplayProperty.Name),
                    Description = e.Description
                }).OrderBy(p => p.Date)
                .ToListAsync();
        }
    }
}
