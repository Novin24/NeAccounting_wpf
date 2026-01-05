using Domain.BaseDomain.Menus;
using DomainShared.ViewModels.Menu;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories.Menus
{
    public interface IMenuManager : IBaseRepository<Menu>
    {
        /// <summary>
        /// دریافت لیست منوها
        /// </summary>
        /// <returns></returns>
        Task<List<MenuListDto>> GetMenuList();
    }
}
