using Domain.NovinEntity.Customers;
using DomainShared.ViewModels;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface ICustomerManager : IRepository<Customer>
    {
        Task<List<SuggestBoxViewModel<Guid>>> GetDisplayUser();
    }
}
