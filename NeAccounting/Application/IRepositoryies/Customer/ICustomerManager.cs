using Domain.NovinEntity.Customers;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Customer;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface ICustomerManager : IRepository<Customer>
    {
        Task<List<SuggestBoxViewModel<Guid, long>>> GetDisplayUser(bool includeDeArchive = false, bool? seller = null, bool? buyer = null);
        Task<List<CustomerListDto>> GetCustomerList(string name, string nationalCode, string mobile);
        
        /// <summary>
        /// لیست برای خروجی گرفتن از کاربران
        /// </summary>
        /// <param name="IsArchive"></param>
        /// <returns></returns>
		Task<List<ExporteCustomerListDto>> GetExporteCustomerList(bool IsArchive);

		Task<(string error, CustomerListDto cus)> GetCustomerById(Guid Id);

        Task<(string error, bool isSuccess, bool Show)> CreateCustomer(string name,
            string mobile,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller);

        Task<(string error, bool isSuccess)> ArchiveCustomer(
            Guid Id,
            bool isActive);

        Task<(string error, bool isSuccess)> UpdateCustomer(
            Guid Id,
            string name,
            string mobile,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller);

        Task<(string error, bool isSuccess)> AddAllCusInNewYear(List<CustomerListDto> cusList);
    }
}
