using Domain.NovinEntity.Materials;
using DomainShared.ViewModels.Pun;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class MaterialManager(NovinDbContext context) : Repository<Material>(context), IMaterialManager
    {
        public Task<List<MatListDto>> GetMaterails()
        {

            return TableNoTracking.Where(t => t.IsActive).Select(x => new MatListDto
            {
                Id = x.Id,
                MaterialName = x.Name,
                Entity = x.Entity,
                LastSellPrice = x.LastSellPrice,
                LastBuyPrice = x.LastBuyPrice,
                UnitName = x.Unit.Name,
                IsService = x.IsService,
            }).ToListAsync();
        }

        public async Task<List<PunListDto>> GetMaterails(string name, string serial)
        {
            var list = await TableNoTracking
                .Include(x => x.Unit)
                .Where(x => string.IsNullOrEmpty(name) || x.Name.Contains(name))
                .Where(x => string.IsNullOrEmpty(serial) || x.Serial.Contains(serial))
                .Select(x => new PunListDto
                {
                    IsActive = x.IsActive,
                    Id = x.Id,
                    MaterialName = x.Name,
                    Serial = x.Serial,
                    IsServise = x.IsService,
                    Address = x.PhysicalAddress,
                    IsManufacturedGoods = x.IsManufacturedGoods,
                    Entity = x.Entity,
                    SEntity = x.Entity.ToString("N0"),
                    UnitId = x.UnitId,
                    LastSellPrice = x.LastSellPrice,
                    LastBuyPrice = x.LastBuyPrice,
                    UnitName = x.Unit.Name
                }).ToListAsync();

            list.ForEach(t => { if (t.Entity == 0) t.SEntity = string.Empty; });
            return list;
        }

        public async Task<(string error, bool isSuccess)> CreateMaterial(string name,
            int unitId,
            bool isService,
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
                                isService,
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
                mt.LastSellPrice = lastPrice;
                mt.PhysicalAddress = address;
                mt.IsManufacturedGoods = isManufacturedGoods;

                Entities.Update(mt);
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
                LastSellPrice = mt.LastSellPrice,
                UnitName = mt.Unit.Name
            });
        }

        public async Task<(string errore, bool isSuccess)> UpdateMaterialEntity(int materialId,
            double entity,
            bool sellOrBuy,
            long? lastPrice = null)
        {
            try
            {
                var mt = await Entities.FindAsync(materialId);

                if (mt == null)
                    return new("کالای مورد نظر یافت نشد !!!", false);

                if (mt.IsService)
                {
                    return new(string.Empty, true);
                }

                if (!sellOrBuy && mt.Entity < entity)
                    return new("موجودی منفی میشود!!!", false);

                if (sellOrBuy)
                    mt.Entity += entity;
                else mt.Entity -= entity;

                if (lastPrice != null && !sellOrBuy)
                {
                    mt.LastSellPrice = lastPrice.Value;
                }

                if (lastPrice != null && sellOrBuy)
                {
                    mt.LastBuyPrice = lastPrice.Value;
                }

                Entities.Update(mt);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> ChangeStatus(
           int id, bool active)
        {
            var unit = await Entities.FirstOrDefaultAsync(t => t.Id == id);
            if (unit == null)
            {
                return new("مورد مدنظر یافت نشد!!", false);
            }
            unit.IsActive = active;

            try
            {
                var t = Entities.Update(unit);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        //public async Task<(string error, bool isSuccess)> AddAllMaterialsInNewYear(List<PunListDto> matList)
        //{
        //    var materialList = matList.Select(t=> new Material())
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}


