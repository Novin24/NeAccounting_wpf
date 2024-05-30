using Domain.NovinEntity.Materials;
using DomainShared.ViewModels.Pun;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class MaterialManager(NovinDbContext context) : Repository<Pun>(context), IMaterialManager
    {
        public async Task<List<MatListDto>> GetMaterails()
        {
            //var sss = await Entities.Include(t => t.RawMaterials).ToListAsync();

            //var rr = sss.First(t => t.Id == Guid.Parse("5e3c0e59-5a45-4eed-b80a-ae6c6c1f9314"));
            //var rn = sss.First(t => t.Id == Guid.Parse("5e3c0e59-5a45-4eed-b80a-ae6c6c9f9314"));
            //var pr = sss.First(t => t.Id == Guid.Parse("71cd42c4-5f0f-ef11-8a52-581122929fa3"));
            //pr.RawMaterials.Add(new PunProduct(7, 88, 3, rr));
            ////pr.RawMaterials.Add(new PunProduct(pr.Id, Guid.Parse("5e3c0e59-5a45-4eed-b80a-ae6c6c1f9314"), 7, 12, .2));
            //Entities.Update(pr);
            //await DbContext.SaveChangesAsync();
            var t = await TableNoTracking.Include(t => t.RawMaterials)
                .Where(t => t.IsActive)
                .Select(x => new MatListDto
                {
                    Id = x.Id,
                    MaterialName = x.Name,
                    Entity = x.Entity,
                    LastSellPrice = x.LastSellPrice,
                    LastBuyPrice = x.LastBuyPrice,
                    UnitName = x.Unit.Name,
                    IsManufacturedGoods = x.IsManufacturedGoods,
                    RawMaterials = x.RawMaterials.Select(t => new RawMaterial()
                    {
                        Ratio = t.Ratio,
                        MaterialId = t.RawMaterialId,
                        MaterialName = t.RawMaterial.Name,
                        UnitName = t.RawMaterial.Unit.Name,
                        UsagePercentage = t.UsagePercentage,
                        WastePercentage = t.WastePercentage,
                    }).ToList(),
                    IsService = x.IsService,
                }).ToListAsync();

            return t;
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
            Guid unitId,
            bool isService,
            long lastPrice,
            string serial,
            string address,
            bool isManufacturedGoods,
            bool isRawMaterial,
            List<RawMaterial> rawMaterials)
        {

            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false);

            List<PunProduct> pnp = [];
            if (isManufacturedGoods)
            {
                var mats = await Entities.Where(t => rawMaterials.Select(t => t.MaterialId).Contains(t.Id)).ToListAsync();
                foreach (var mat in rawMaterials)
                {
                    pnp.Add(new PunProduct(mat.WastePercentage, mat.UsagePercentage, mat.Ratio, mats.First(t => t.Id == mat.MaterialId)));
                }
            }

            try
            {
                await Entities.AddAsync(new Pun(name,
                                unitId,
                                isService,
                                lastPrice,
                                serial,
                                address,
                                isManufacturedGoods,
                                isRawMaterial,
                                pnp));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(69t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateMaterial(
            Guid materialId,
            string name,
            Guid unitId,
            string serial,
            string address,
            long lastPrice,
            bool isManufacturedGoods,
            bool isRawMaterial,
            List<RawMaterial> rawMaterials)
        {

            if (await TableNoTracking.AnyAsync(t => t.Name == name && t.Id != materialId))
                return new("کاربر گرامی این کالا از قبل تعریف شده می‌باشد!!!", false);

            var mt = await Entities.FindAsync(materialId);

            if (mt == null)
                return new("کالای مورد نظر یافت نشد !!!", false);

            List<PunProduct> pnp = [];

            if (isManufacturedGoods)
            {
                var mats = await Entities.Where(t => rawMaterials.Select(t => t.MaterialId).Contains(t.Id)).ToListAsync();
                foreach (var mat in rawMaterials)
                {
                    pnp.Add(new PunProduct(mat.WastePercentage, mat.UsagePercentage, mat.Ratio, mats.First(t => t.Id == mat.MaterialId)));
                }
            }

            try
            {
                mt.SetName(name);
                mt.SetSerial(serial);
                mt.SetAddress(address);
                mt.RawMaterials.Clear();
                mt.SetRawMaterials(pnp);
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
    }
}


