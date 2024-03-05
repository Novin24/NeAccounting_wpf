using Domain.NovinEntity.Expense;
using DomainShared.Enums;
using DomainShared.ViewModels.Expense;
using Infrastructure.EntityFramework;
using NeApplication.IRepositoryies.Expens;

namespace Infrastructure.Repositories
{
    public class ExpenseManager : Repository<Expense>, IExpenseManager
    {
        public ExpenseManager(NovinDbContext context) : base(context) { }

        public Task<(string error, bool isSuccess)> CreateExpense(DateTime submitDate, string Expensetype, long Amount, PaymentType PayType, string Receiver, string Description)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExpenselistDto>> GetExpenselist(DateTime? startDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }
    }
}
