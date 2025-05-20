using Domain.Common;
using Domain.NovinEntity.Documents;
using Domain.NovinEntity.Materials;
using DomainShared.Errore;

namespace Domain.Enities.NovinEntity.Remittances
{
    /// <summary>
    /// حواله خرید
    /// </summary>
    public class BuyRemittance : LocalEntity<Guid>
    {
        #region Navigation

        /// <summary>
        /// اجناس
        /// </summary>
        public Pun Material { get; private set; }
        public Guid MaterialId { get; set; }

        /// <summary>
        /// اسناد
        /// </summary>
        public Document Document { get; private set; }
        public Guid DocumentId { get; private set; }
        #endregion

        #region Properties

        /// <summary>
        /// قیمت
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// توضیح
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// قیمت کل
        /// </summary>
        public long TotalPrice { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public double AmountOf { get; set; }
        #endregion

        #region Ctor
        internal BuyRemittance() { }

        public BuyRemittance(
            Guid materialId,
            double amountOf,
            long price,
            long toatalPrice,
            DateTime submitDate,
            string? descripion)
        {
            SetDesc(descripion);
            MaterialId = materialId;
            TotalPrice = toatalPrice;
            AmountOf = amountOf;
            Price = price;
            SubmitDate = submitDate;
        }
        #endregion

        #region Methods
        public BuyRemittance SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 100)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "صد"));

            Description = description;
            return this;
        }
        #endregion
    }
}
