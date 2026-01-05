using Domain.BaseDomain.User;
using Domain.Common;

namespace Domain.BaseDomain.Menus
{
    public class Menu : BaseEntity<Guid>
    {
        #region Navigation
        public ICollection<IdentityUser> Users { get; set; } = [];
        public Guid? ParentId { get; set; }
        #endregion

        #region ctor
        public Menu()
        {
            
        }
        #endregion


        #region Properties
        public string Name { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Level { get; set; } = default!;
        public bool IsDefault { get; set; } = false;
        public byte Root { get; set; } = default!;
        #endregion
    }
}
