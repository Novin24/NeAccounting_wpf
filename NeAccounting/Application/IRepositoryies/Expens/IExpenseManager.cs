using Domain.NovinEntity.Expense;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Expense;
using NeApplication.Common;

namespace NeApplication.IRepositoryies.Expens
{
    public interface IExpenseManager : IRepository<Expense>
    {
        Task<List<ExpenselistDto>> GetExpenselist(DateTime? startDate, DateTime? endDate);
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
