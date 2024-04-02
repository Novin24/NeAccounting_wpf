using Domain.NovinEntity.Expense;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.ViewModels.Expense;
using DomainShared.ViewModels.PagedResul;
using NeApplication.Common;

namespace NeApplication.IRepositoryies.Expens
{
    public interface IExpenseManager : IRepository<Expense>
    {
        Task<PagedResulViewModel<ExpenselistDto>> GetExpenselist(DateTime? startDate,
            DateTime? endDate,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> CreateExpense(
            DateTime submitDate,
            string Expensetype,
            long Amount,
            PaymentType PayType,
            string Receiver,
            string Description);
        Task<(string error, bool isSuccess)> UpdateExpense(Guid ExpenseID, DateTime submitDate, string Expensetype, long Amount, PaymentType PayType, string Receiver, string Description);
    }
}
