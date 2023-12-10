using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
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
                .Where(x => string.IsNullOrEmpty(name) || x.Name.Contains(name))
                .Where(x => string.IsNullOrEmpty(serial) || x.Serial.Contains(serial))
                .Where(t => !t.IsDeleted)
                .Select(x => new PunListDto
                {
                    Id = x.Id,
                    MaterialName = x.Name,
                    Serial = x.Serial,
                    SellPrice = x.SellPrice,
                    Address = x.PhysicalAddress,
                    BuyPrice = x.BuyPrice,
                    Entity = x.Entity,
                    LastPrice = x.LastPrice,
                    UnitName = x.Unit.Name
                }).ToListAsync();
        }


        public async Task<(string error, bool isSuccess)> CreateMaterial(string name,
            double entity,
            int unitId,
            string serial,
            string address)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false);

            try
            {

                var t = await Entities.AddAsync(new Material(name,
                                unitId,
                                serial,
                                entity,
                                address));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateMaterial(
            int materialId,
            string name,
            double entity,
            int unitId,
            string serial,
            string address)
        {
            try
            {
                var mt = await Entities.FindAsync(materialId);

                if (mt == null)
                    return new("کالای مورد نظر یافت نشد !!!", false);

                mt.Name = name;
                mt.Entity = entity;
                mt.UnitId = unitId;
                mt.Serial = serial;
                mt.PhysicalAddress = address;

                Update(mt);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

    }
}
