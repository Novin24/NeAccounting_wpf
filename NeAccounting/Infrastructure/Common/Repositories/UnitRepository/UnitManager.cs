using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.unit;
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

        public Task<List<UnitListDto>> GetUnitList()
        {
            return TableNoTracking.Select(x => new UnitListDto
            {
                Id = x.Id,
                UnitName = x.Name,
                IsActive = x.IsActive,
                Description = x.Descrip

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
                var t = await Entities.AddAsync(new Unit(name, description));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
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

            unit.Name = name;
            unit.Descrip = description;

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
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
    }
}
