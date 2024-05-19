using Domain.NovinEntity.Expense;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.ViewModels.Expense;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies.Expens;

namespace Infrastructure.Repositories
{
    public class ExpenseManager(NovinDbContext context) : Repository<Expense>(context), IExpenseManager
    {
        public async Task<(string error, bool isSuccess)> CreateExpense(DateTime submitDate, string expensetype, long amount, PaymentType payType, string receiver, string description)
        {
            //if (await TableNoTracking.AnyAsync(t => t.Expensetype == expensetype))
            //    return new("کاربر گرامی این هزینه از قبل تعریف شده می‌باشد!!!", false);

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
                return new(" خطا در اتصال به پایگاه داده code(59t46993)!!!", false);
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

                Entities.Update(x);
            }
            catch (Exception ex)
            {
                return new(" خطا در اتصال به پایگاه داده code(59t46993)!!!", false);
            }
            return new(string.Empty, true);
        }
        public async Task<PagedResulViewModel<ExpenselistDto>> GetExpenselist(DateTime? startDate,
            DateTime? endDate,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            var query = TableNoTracking
                .Where(x => !startDate.HasValue || x.SubmitDate >= startDate)
                .Where(x => !endDate.HasValue || x.SubmitDate < endDate)
                .Select(e => new ExpenselistDto()
                {
                    Id = e.Id,
                    Receiver = e.Receiver,
                    Date = e.SubmitDate,
                    Type = e.PayType,
                    Expensetype = e.Expensetype,
                    Price = e.Amount,
                    Description = e.Description,
                }).OrderBy(p => p.Date)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            if (isInit && totalCount != 0)
            {
                pageNum = totalCount / pageCount;
                if (totalCount % pageCount != 0)
                {
                    pageNum++;
                }
            }
            var li = await query.Skip((pageNum - 1) * pageCount).Take(pageCount).ToListAsync();

            return new PagedResulViewModel<ExpenselistDto>(totalCount, pageCount, pageNum, li);
        }

        public async Task<long> GetTotalExp()
        {
            return await TableNoTracking.SumAsync(t => t.Amount);
        }
    }
}
