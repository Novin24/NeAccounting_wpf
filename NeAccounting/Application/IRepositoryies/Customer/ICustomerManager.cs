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

        /// <summary>
        /// لیست کاربردی
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nationalCode"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<List<CustomerListDto>> GetCustomerList(string name, string nationalCode, string mobile);
        
        /// <summary>
        /// لیست برای خروجی گرفتن از کاربران
        /// </summary>
        /// <param name="IsArchive"></param>
        /// <returns></returns>
		Task<List<ExporteCustomerListDto>> GetExporteCustomerList(bool IsArchive);

		Task<(string error, CustomerListDto cus)> GetCustomerById(Guid Id);

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="cashCredit"></param>
        /// <param name="promissoryNote"></param>
        /// <param name="nationalCode"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="havePromissoryNote"></param>
        /// <param name="haveCashCredit"></param>
        /// <param name="isBuyer"></param>
        /// <param name="isSeller"></param>
        /// <returns></returns>
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

        /// <summary>
        /// فعال یا غیرفعال کردن
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> ArchiveCustomer(
            Guid Id,
            bool isActive);

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="cashCredit"></param>
        /// <param name="promissoryNote"></param>
        /// <param name="nationalCode"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="havePromissoryNote"></param>
        /// <param name="haveCashCredit"></param>
        /// <param name="isBuyer"></param>
        /// <param name="isSeller"></param>
        /// <returns></returns>
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


		/// <summary>
		/// اضافه کردن همه به سال مالی جدید
		/// </summary>
		/// <param name="cusList"></param>
		/// <returns></returns>
		Task<(string error, bool isSuccess)> AddAllCusInNewYear(List<CustomerListDto> cusList);
    }
}
