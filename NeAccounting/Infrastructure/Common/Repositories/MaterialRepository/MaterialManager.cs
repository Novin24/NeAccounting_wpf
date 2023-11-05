using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies.Materials;

namespace Infrastructure.Repositories.MaterialRepository
{
    public class MaterialManager : Repository<Material>, IMaterialManager
    {
        public MaterialManager(NovinDbContext context) : base(context) { }

        public Task<List<SuggestBoxViewModel<int>>> GetMaterails()
        {

            return TableNoTracking.Select(x => new SuggestBoxViewModel<int>
            {
                Id = x.Id,
                DisplayName = x.Name

            }).ToListAsync();
        }
    }


}
