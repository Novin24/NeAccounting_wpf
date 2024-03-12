using Domain.Common;

namespace Domain.NovinEntity.Materials
{
    public class Unit : LocalEntity
    {
        #region Navigation
        public ICollection<Material> Materials { get; private set; }
        #endregion

        #region Property
        public string Name { get;  set; }
        public string Descrip { get;  set; }
        public bool IsActive { get;  set; }
        #endregion

        #region ctor
        internal Unit()
        {

        }

        public Unit(string name,
            string description)
        {
            Name = name;
            Descrip = description;
            IsActive = true;
        }
        #endregion
    }
}