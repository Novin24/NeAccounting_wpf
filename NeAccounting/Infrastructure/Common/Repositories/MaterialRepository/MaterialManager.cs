using Domain.NovinEntity.Materials;
using DomainShared.Pun;
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

        public Task<List<PunListDto>> GetMaterails(string name, string serial)
        {
            return TableNoTracking
                .Include(x => x.Unit)
                .Where(x => !string.IsNullOrEmpty(name) && x.Name.Contains(name))
                .Where(x => !string.IsNullOrEmpty(serial) && x.Name.Contains(serial))
                .Select(x => new PunListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Serial = x.Serial,
                    SellPrice = x.SellPrice,
                    Address = x.PhysicalAddress,
                    BuyPrice = x.BuyPrice,
                    Entity = x.Entity,
                    LastPrice = x.LastPrice,
                    UnitName = x.Unit.Name
                }).ToListAsync();
        }
    }


}
