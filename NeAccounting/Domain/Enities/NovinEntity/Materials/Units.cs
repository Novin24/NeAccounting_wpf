using Domain.Common;

namespace Domain.NovinEntity.Materials
{
    public class Units : LocalEntity<Guid>
    {
        #region Navigation
        public ICollection<Pun> Materials { get; private set; }
        #endregion

        #region Property
        public string Name { get; set; }
        public string? Descrip { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region ctor
        internal Units()
        {

        }

        public Units(string name,
            string description)
        {
            Name = name;
            Descrip = description;
            IsActive = true;
        }
        #endregion
    }
}