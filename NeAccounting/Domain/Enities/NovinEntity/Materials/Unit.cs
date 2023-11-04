using Domain.Common;

namespace Domain.NovinEntity.Materials
{
    public class Unit : LocalEntity
    {
        #region Navigation
        #endregion

        #region Property
        public string Name { get; private set; }
        public string Descrip { get; private set; }
        public bool Active { get; private set; }
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
            Active = true;
        }
        #endregion
    }
}