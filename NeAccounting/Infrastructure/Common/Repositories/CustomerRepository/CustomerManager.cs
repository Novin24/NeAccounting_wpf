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
    public class CustomerManager(NovinDbContext context) : Repository<Customer>(context), ICustomerManager
    {
        public Task<List<SuggestBoxViewModel<Guid, long>>> GetDisplayUser(bool includeDeArchive = false, bool? seller = null, bool? buyer = null)
        {
            return TableNoTracking.Where(t => seller == null || t.Seller)
                .Where(b => buyer == null || b.Buyer)
                .Where(b => includeDeArchive || b.IsActive)
                .Where(c => c.Id != Guid.Empty)
                .Select(x => new SuggestBoxViewModel<Guid, long>
                {
                    Id = x.Id,
                    DisplayName = x.Name,
                    UniqNumber = x.CusId,
                    TotalValidity = x.TotalCredit
                }).OrderBy(c=> c.DisplayName).ToListAsync();
		}

		public Task<List<ExporteCustomerListDto>> GetExporteCustomerList(bool IsArchive)
		{
            return TableNoTracking
                .Where(t => IsArchive == true || t.IsActive != IsArchive)
				.Where(c => c.Id != Guid.Empty)
				.Select(t => new ExporteCustomerListDto
                {
                    Name = t.Name,
                    NationalCode = t.NationalCode,
					CusTypeName = t.Type.ToDisplay(DisplayProperty.Name),
                    Mobile = t.Mobile,
                    Buyer = t.Buyer,
                    Seller = t.Seller,
                    Address = t.Address,
				}).OrderBy(t => t.Name).ToListAsync();
		}

		public Task<List<CustomerListDto>> GetCustomerList(string name, string nationalCode, string mobile)
        {
            return TableNoTracking
                .Where(x => string.IsNullOrEmpty(name) || x.Name.Contains(name))
                .Where(x => string.IsNullOrEmpty(nationalCode) || x.NationalCode.Contains(nationalCode))
                .Where(x => string.IsNullOrEmpty(mobile) || x.Mobile.Contains(mobile))
                .Where(c => c.Id != Guid.Empty)
                .Select(x => new CustomerListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    NationalCode = x.NationalCode,
                    IsActive = x.IsActive,
                    Mobile = x.Mobile,
                    Seller = x.Seller,
                    Buyer = x.Buyer,
                    Address = x.Address,
                    CashCredit = x.CashCredit,
                    ChequeCredit = x.ChequeCredit,
                    HaveCashCredit = x.HaveCashCredit,
                    HaveChequeGuarantee = x.HaveChequeGuarantee,
                    HavePromissoryNote = x.HavePromissoryNote,
                    UniqNumber = x.CusId,
                    PromissoryNote = x.PromissoryNote,
                    TTotalCredit = x.TotalCredit,
                    TotalCredit = x.TotalCredit.ToString("N0"),
                    CusTypeName = x.Type.ToDisplay(DisplayProperty.Name),
                    CusType = x.Type,
                }).OrderBy(t=> t.Name).ToListAsync();
        }


        public async Task<(string error, bool isSuccess)> CreateCustomer(string name,
            string mobile,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این مشتری از قبل تعریف شده می‌باشد!!!", false);

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
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(07t43493)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateCustomer(
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
            bool isSeller)
        {
            try
            {
                var mt = await Entities.FindAsync(Id);

                if (mt == null)
                    return new("مشتری مورد نظر یافت نشد !!!", false);

                mt.SetName(name);
                mt.SetNationalCode(nationalCode);
                mt.SetMobile(mobile);
                mt.SetAddress(address);
                mt.Seller = isSeller;
                mt.Buyer = isBuyer;
                mt.CashCredit = cashCredit;
                mt.HaveCashCredit = haveCashCredit;
                mt.HavePromissoryNote = havePromissoryNote;
                mt.PromissoryNote = promissoryNote;
                mt.TotalCredit = promissoryNote + mt.ChequeCredit + cashCredit;
                mt.Type = type;

                Entities.Update(mt);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(07t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> ArchiveCustomer(
            Guid Id,
            bool isActive)
        {
            try
            {
                var cus = await Entities.FindAsync(Id);

                if (cus == null)
                    return new("مشتری مورد نظر یافت نشد !!!", false);

                cus.IsActive = isActive;
                Entities.Update(cus);
            }
            catch 
            {
                return new(" خطا در اتصال به پایگاه داده code(17t46993)!!!", false);
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
                UniqNumber = mt.CusId,
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
                TotalCredit = mt.TotalCredit.ToString("N0"),
                CusType = mt.Type,
            });
        }


        public async Task<(string error, bool isSuccess)> AddAllCusInNewYear(List<CustomerListDto> cusList)
        {
            var units = cusList.Select(t => new Customer(t.Name,
                t.Mobile,
                t.TTotalCredit,
                t.ChequeCredit,
                t.PromissoryNote,
                t.NationalCode,
                t.Address,
                t.CusType,
                t.HavePromissoryNote,
                t.HaveCashCredit,
                t.Buyer,
                t.Seller,
                t.Id));

            try
            {
                await Entities.AddRangeAsync(units);
            }
            catch (Exception)
            {
                return new(" خطا در اتصال به پایگاه داده code(27t46993)!!!", false);
            }
            return new(string.Empty, true);
        }
	}
}
