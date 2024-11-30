using Domain.Common;
using DomainShared.Errore;

namespace Domain.NovinEntity.Materials
{
    public class Units : LocalEntity<Guid>
    {
        #region Navigation
        public ICollection<Pun> Materials { get; private set; }
        #endregion

        #region Property

        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Descrip { get;private set; }

		/// <summary>
		/// فعال بودن
		/// </summary>
		public bool IsActive { get; set; }


        /// <summary>
        /// شماره واحد
        /// </summary>
		public int IdNumber { get; set; }
		#endregion

		#region ctor
		internal Units()
        {

        }

        public Units(string name,
            string description)
        {
            SetName(name);
            SetDesc(description);
            IsActive = true;
        }

		/// <summary>
		/// اضافه کردن همه به سال مالی جدید
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="id"></param>
		public Units(
            string name,
            string description,
            Guid id) : this(name, description)
        {
            Id = id;
        }
        #endregion

        #region Methods
        public Units SetName(string name)
        {
            if (name.Length > 30)
                throw new ArgumentException(NeErrorCodes.IsLess("نام واحد", "سی"));

            Name = name;
            return this;
        }

        public Units SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 100)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "صد"));

            Descrip = description;
            return this;
        }
        #endregion
    }
}