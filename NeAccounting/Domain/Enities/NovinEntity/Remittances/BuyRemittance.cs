using Domain.Common;
using Domain.NovinEntity.Documents;
using Domain.NovinEntity.Materials;

namespace Domain.Enities.NovinEntity.Remittances
{
    public class BuyRemittance : LocalEntity<Guid>
    {
        #region Navigation
        public Material Material { get; private set; }
        public int MaterialId { get;  set; }
        public Document Document { get; private set; }
        public Guid DocumentId { get; private set; }
        #endregion

        #region Ctor
        internal BuyRemittance() { }

        public BuyRemittance(
            int materialId,
            double amountOf,
            long price,
            long toatalPrice,
            DateTime submitDate,
            string? descripion)
        {
            MaterialId = materialId;
            TotalPrice = toatalPrice;
            AmountOf = amountOf;
            Price = price;
            Description = descripion;
            SubmitDate = submitDate;
        }
        #endregion

        #region Properties
        public long Price { get; set; }
        public string? Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public long TotalPrice { get; set; }
        public double AmountOf { get; set; }
        #endregion
    }
}
