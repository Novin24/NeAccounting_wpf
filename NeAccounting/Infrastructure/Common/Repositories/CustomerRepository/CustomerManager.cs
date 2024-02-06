using Domain.NovinEntity.Customers;
using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Customer;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class CustomerManager : Repository<Customer>, ICustomerManager
    {
        public CustomerManager(NovinDbContext context) : base(context) { }


        public Task<List<SuggestBoxViewModel<Guid,long>>> GetDisplayUser(bool? seller = null, bool? buyer = null)
        {
            return TableNoTracking.Where(t => seller == null || t.Seller)
                .Where(b=> buyer == null || b.Buyer)
                .Select(x => new SuggestBoxViewModel<Guid, long>
                {
                    Id = x.Id,
                    DisplayName = x.Name,
                    UniqNumber = x.CusId
                }).ToListAsync();
        }

        public Task<List<CustomerListDto>> GetCustomerList(string name, string nationalCode, string mobile)
        {
            return TableNoTracking
                .Where(x => string.IsNullOrEmpty(name) || x.Name.Contains(name))
                .Where(x => string.IsNullOrEmpty(nationalCode) || x.NationalCode.Contains(nationalCode))
                .Where(x => string.IsNullOrEmpty(mobile) || x.Mobile.Contains(mobile))
                .Select(x => new CustomerListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    NationalCode = x.NationalCode,
                    Mobile = x.Mobile,
                    Seller = x.Seller,
                    Buyer = x.Buyer,
                    Address = x.Address,
                    CashCredit = x.CashCredit,
                    ChequeCredit = x.ChequeCredit,
                    HaveCashCredit = x.HaveCashCredit,
                    HaveChequeGuarantee = x.HaveChequeGuarantee,
                    HavePromissoryNote = x.HavePromissoryNote,
                    PromissoryNote = x.PromissoryNote,
                    TotalCredit = x.TotalCredit,
                    CusTypeName = x.Type.ToDisplay(DisplayProperty.Name),
                    CusType = x.Type,
                }).ToListAsync();
        }


        public async Task<(string error, bool isSuccess)> CreateCustomer(string name,
            string mobile,
            uint cashCredit,
            uint promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false);

            try
            {

                var t = await Entities.AddAsync(new Customer(
                    name,
                    mobile,
                    cashCredit + promissoryNote,
                    cashCredit,
                    promissoryNote,
                    nationalCode,
                    address,
                    type,
                    havePromissoryNote,
                    haveCashCredit,
                    isBuyer,
                    isSeller));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateCustomer(
            Guid Id,
            string name,
            string mobile,
            uint cashCredit,
            uint promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            try
            {
                var mt = await Entities.FindAsync(Id);

                if (mt == null)
                    return new("کالای مورد نظر یافت نشد !!!", false);

                mt.Name = name;
                mt.NationalCode = nationalCode;
                mt.Mobile = mobile;
                mt.Seller = isSeller;
                mt.Buyer = isBuyer;
                mt.Address = address;
                mt.CashCredit = cashCredit;
                mt.HaveCashCredit = haveCashCredit;
                mt.HavePromissoryNote = havePromissoryNote;
                mt.PromissoryNote = promissoryNote;
                mt.TotalCredit = promissoryNote + mt.ChequeCredit + cashCredit;
                mt.Type = type;

                Update(mt, false);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, CustomerListDto cus)> GetCustomerById(Guid Id)
        {
            var mt = await TableNoTracking
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (mt == null)
                return new("مشتری مورد نظر یافت نشد !!!", new CustomerListDto());

            return new(string.Empty, new CustomerListDto()
            {
                Id = mt.Id,
                Name = mt.Name,
                NationalCode = mt.NationalCode,
                Mobile = mt.Mobile,
                Seller = mt.Seller,
                Buyer = mt.Buyer,
                Address = mt.Address,
                CashCredit = mt.CashCredit,
                ChequeCredit = mt.ChequeCredit,
                HaveCashCredit = mt.HaveCashCredit,
                HaveChequeGuarantee = mt.HaveChequeGuarantee,
                HavePromissoryNote = mt.HavePromissoryNote,
                PromissoryNote = mt.PromissoryNote,
                TotalCredit = mt.TotalCredit,
                CusType = mt.Type,
            });
        }

    }
}
