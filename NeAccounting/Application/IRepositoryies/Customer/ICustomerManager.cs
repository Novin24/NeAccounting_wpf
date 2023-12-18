using Domain.NovinEntity.Customers;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface ICustomerManager : IRepository<Customer>
    {
        Task<List<SuggestBoxViewModel<Guid>>> GetDisplayUser();
        Task<List<CustomerListDto>> GetCustomerList(string name, string nationalCode, string mobile);
        Task<(string error, CustomerListDto cus)> GetCustomerById(Guid Id);
        Task<(string error, bool isSuccess)> CreateCustomer(string name,
            string mobile,
            uint totalCredit,
            uint chequeCredit,
            uint cashCredit,
            uint promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveChequeGuarantee,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller);

        Task<(string error, bool isSuccess)> UpdateCustomer(
            Guid Id,
            string name,
            string mobile,
            uint totalCredit,
            uint chequeCredit,
            uint cashCredit,
            uint promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveChequeGuarantee,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller);
    }
}
