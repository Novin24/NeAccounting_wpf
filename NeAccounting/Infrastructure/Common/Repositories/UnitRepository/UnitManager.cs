using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class UnitManager : Repository<Unit>, IUnitManager
    {
        public UnitManager(NovinDbContext dbContext) : base(dbContext)
        {

        }

        public Task<List<SuggestBoxViewModel<int>>> GetUnits()
        {
            return TableNoTracking.Select(x => new SuggestBoxViewModel<int>
            {
                Id = x.Id,
                DisplayName = x.Name

            }).ToListAsync();
        }
    }
}
