using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using DomainShared.ViewModels.unit;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class UnitManager : Repository<Units>, IUnitManager
    {
        public UnitManager(NovinDbContext dbContext) : base(dbContext)
        {

        }

        public Task<List<UnitListDto>> GetUnitList()
        {
            return TableNoTracking.Select(x => new UnitListDto
            {
                Id = x.Id,
                UnitName = x.Name,
                IsActive = x.IsActive,
                Description = x.Descrip,
                IdNumber = x.IdNumber,

            }).ToListAsync();
        }

        public Task<List<SuggestBoxViewModel<Guid>>> GetUnits(bool IgnorArchive = false)
        {
            return TableNoTracking.Where(t => IgnorArchive || t.IsActive)
                .Select(x => new SuggestBoxViewModel<Guid>
                {
                    Id = x.Id,
                    DisplayName = x.Name
                }).ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> CreateUnit(string name,
            string description)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name))
                return new("کاربر گرامی این واحد از قبل تعریف شده می‌باشد!!!", false);

            try
            {
                await Entities.AddAsync(new Units(name, description));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(10t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateUnit(
            Guid id,
            string name,
            string description)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name && t.Id != id))
                return new("کاربر گرامی این واحد از قبل تعریف شده می‌باشد!!!", false);

            var unit = await Table.FirstOrDefaultAsync(t => t.Id == id);
            if (unit == null)
            {
                return new("واحد مورد نظر یافت نشد!!", false);
            }

            try
            {
                unit.SetName(name);
                unit.SetDesc(description);

                Entities.Update(unit);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(20t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> ChangeStatus(
            Guid id, bool active)
        {
            var unit = await Entities.FirstOrDefaultAsync(t => t.Id == id);
            if (unit == null)
            {
                return new("واحد مورد نظر یافت نشد!!", false);
            }
            unit.IsActive = active;

            try
            {
                var t = Entities.Update(unit);
            }
            catch (Exception ex)
            {
                return new(" خطا در اتصال به پایگاه داده code(30t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> AddAllUnitsInNewYear(List<UnitListDto> unitList)
        {
            var units = unitList.Select(t => new Units(t.UnitName, t.Description, t.Id));
            try
            {
                await Entities.AddRangeAsync(units);
            }
            catch (Exception)
            {
                return new(" خطا در اتصال به پایگاه داده code(40t46993)!!!", false);
            }
            return new(string.Empty, true);
        }
		public async Task<(string error, Guid unitId)> GetUnitIdByName(string unitName)
		{
			var unit = await TableNoTracking.FirstOrDefaultAsync(t => t.Name == unitName);
			if (unit == null)
			{
				return new("واحد مورد نظر یافت نشد!!", Guid.Empty);
			}
			return new(string.Empty, unit.Id);
		}
	}
}
