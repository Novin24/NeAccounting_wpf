using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class MaterialManager(NovinDbContext context) : Repository<Material>(context), IMaterialManager
    {
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
                .Select(x => new PunListDto
                {
                    Id = x.Id,
                    MaterialName = x.Name,
                    Serial = x.Serial,
                    Address = x.PhysicalAddress,
                    IsManufacturedGoods = x.IsManufacturedGoods,
                    Entity = x.Entity,
                    UnitId = x.UnitId,
                    LastPrice = x.LastPrice,
                    UnitName = x.Unit.Name
                }).ToListAsync();
        }


        public async Task<(string error, bool isSuccess)> CreateMaterial(string name,
            int unitId,
            long lastPrice,
            string serial,
            string address,
            bool isManufacturedGoods)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false);

            try
            {

                var t = await Entities.AddAsync(new Material(name,
                                unitId,
                                lastPrice,
                                serial,
                                address,
                                isManufacturedGoods));
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
            int unitId,
            string serial,
            string address,
            long lastPrice,
            bool isManufacturedGoods)
        {
            try
            {
                var mt = await Entities.FindAsync(materialId);

                if (mt == null)
                    return new("کالای مورد نظر یافت نشد !!!", false);

                mt.Name = name;
                mt.UnitId = unitId;
                mt.Serial = serial;
                mt.LastPrice = lastPrice;
                mt.PhysicalAddress = address;
                mt.IsManufacturedGoods = isManufacturedGoods;

                Update(mt,false);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, PunListDto pun)> GetMaterailById(int Id)
        {
            var mt = await TableNoTracking
                .Include(t => t.Unit)
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (mt == null)
                return new("کالای مورد نظر یافت نشد !!!", new PunListDto());

            return new(string.Empty, new PunListDto()
            {
                MaterialName = mt.Name,
                Address = mt.PhysicalAddress,
                Serial = mt.Serial,
                IsManufacturedGoods = mt.IsManufacturedGoods,
                Entity = mt.Entity,
                Id = Id,
                LastPrice = mt.LastPrice,
                UnitName = mt.Unit.Name
            });
        }
    }
}
