using Domain.NovinEntity.Materials;
using DomainShared.ViewModels.Pun;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class MaterialManager(NovinDbContext context) : Repository<Pun>(context), IMaterialManager
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

        public async Task<(string error, bool isSuccess, bool Show)> CreateMaterial(string name,
            Guid unitId,
            bool isService,
            long lastPrice,
            string serial,
            string address,
            bool isManufacturedGoods,
            double? miniEntity = null)
		{
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false, false);

            try
            {
                await Entities.AddAsync(new Pun(name,
                               unitId,
                               isService,
                               lastPrice,
							   miniEntity,
							   serial,
                               address,
                               isManufacturedGoods));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(69t46993)!!!", false, true);
            }
            return new(string.Empty, true, false);
        }

        public async Task<(string error, bool isSuccess)> UpdateMaterial(
            Guid materialId,
            string name,
            Guid unitId,
            string serial,
            string address,
            long lastPrice,
            bool isManufacturedGoods)
        {
            var mt = await Entities.FindAsync(materialId);

            if (mt == null)
                return new("کالای مورد نظر یافت نشد !!!", false);

            try
            {
                mt.SetName(name);
                mt.SetSerial(serial);
                mt.SetAddress(address);
                mt.UnitId = unitId;
                mt.LastSellPrice = lastPrice;
                mt.IsManufacturedGoods = isManufacturedGoods;

                Entities.Update(mt);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(79t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, PunListDto pun)> GetMaterailById(Guid Id)
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

        public async Task<(string errore, bool isSuccess)> UpdateMaterialEntity(Guid materialId,
            double entity,
            bool sellOrBuy,
            long? lastPrice = null)
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

            try
            {
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
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(89t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> ChangeStatus(
           Guid id, bool active)
        {
            var unit = await Entities.FirstOrDefaultAsync(t => t.Id == id);
            if (unit == null)
            {
                return new("مورد مدنظر یافت نشد!!", false);
            }

            try
            {
                unit.IsActive = active;
                var t = Entities.Update(unit);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(93t46993)!!!", false);
            }
            return new(string.Empty, true);
		}
		
		public async Task<(string error, bool isSuccess)> AddAllMaterialsInNewYear(List<PunListDto> matList)
        {
            var materialList = matList.Select(t => new Pun(t.Id,
                t.MaterialName,
                t.UnitId,
                t.IsServise,
                t.LastSellPrice,
                t.Serial,
                t.Entity,
                t.LastBuyPrice,
                t.IsActive,
                t.Address));

            try
            {
                await Entities.AddRangeAsync(materialList);
            }
            catch (Exception)
            {

                return new(" خطا در اتصال به پایگاه داده code(00t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

		public Task<List<ExporteMaterialListDto>> GetExporteMaterialList(bool IsArchive)
		{
            return TableNoTracking
                .Where(t => IsArchive == true || t.IsActive != IsArchive)
                .Where(t => t.IsService == false)
                .Select(x => new ExporteMaterialListDto
                {
                    MaterialName = x.Name,
                    LastSellPrice = x.LastSellPrice,
                    UnitName = x.Unit.Name,
                    Address = x.PhysicalAddress,
                    Serial = x.Serial,
                    UnitNumber = x.Unit.IdNumber,
                }).OrderBy(t => t.MaterialName).ToListAsync();
		}
	}
}


