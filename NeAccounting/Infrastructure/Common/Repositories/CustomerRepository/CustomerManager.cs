using Domain.NovinEntity.Customers;
using DomainShared.ViewModels;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class CustomerManager : Repository<Customer>, ICustomerManager
    {
        public CustomerManager(NovinDbContext context) : base(context) { }


        public Task<List<SuggestBoxViewModel<Guid>>> GetDisplayUser()
        {
            return TableNoTracking.Select(x => new SuggestBoxViewModel<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name

            }).ToListAsync();
        }



    }
}
