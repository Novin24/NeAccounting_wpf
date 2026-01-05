using Domain.BaseDomain.Menus;
using DomainShared.ViewModels.Menu;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories.Menus;

namespace Infrastructure.BaseRepositories
{
    public class MenuManager : BaseRepository<Menu>, IMenuManager
    {
        public MenuManager(BaseDomainDbContext context) : base(context) { }

        public async Task<List<MenuListDto>> GetMenuList()
        {
            var menus = await TableNoTracking
                .Where(t => !t.IsDefault )
                .ToListAsync();

            if (menus == null) return [];

            var result = new List<MenuListDto>();

            var rootItems = menus.Where(t => t.Root == 0).OrderBy (t=>t.Level).ToList();

            foreach (var item in rootItems)
            {
                var parentDto = new MenuListDto
                {
                    Id = item.Id,
                    Name = item.DisplayName,
                    IsParent = true
                };

                var children = menus
                    .Where(c => c.ParentId == item.Id)
                    .OrderBy(t=> t.Level)
                    .Select(n => new MenuListDto
                    {
                        Id = n.Id,
                        Name = n.DisplayName,
                        IsParent = false,
                        Parent = parentDto
                    })
                    .ToList();

                parentDto.Children = children;

                result.Add(parentDto);
            }

            return result;

            //return [.. menus
            //    .Where(t => t.Root == 0)
            //    .Select(t => new UserMenuDto
            //    {
            //        Id = t.Id,
            //        Name = t.DisplayName,
            //        IsParent = true,
            //        Children = [.. menus.Where(c => c.ParentId == t.Id).Select(n => new UserMenuDto
            //        {
            //            Name = n.DisplayName,
            //            Id = n.Id,
            //            IsParent = false
            //        })]
            //    })];
        }
    }
}
