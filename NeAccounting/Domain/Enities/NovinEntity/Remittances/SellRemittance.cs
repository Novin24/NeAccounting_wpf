using Domain.Common;
using Domain.NovinEntity.Documents;
using Domain.NovinEntity.Materials;

namespace Domain.Enities.NovinEntity.Remittances
{
    public class SellRemittance : LocalEntity<Guid>
    {
        #region Navigation
        public Material Material { get; private set; }
        public int MaterialId { get; set; }
        public Document Document { get; private set; }
        public Guid DocumentId { get; set; }
        #endregion

        #region Ctor
        internal SellRemittance() { }

        internal SellRemittance(Guid id,
            int materialId,
            Guid documentId,
            double amountOf,
            uint price,
            int toatalPrice,
            DateTime submitDate,
            string descripion)
        {
            Id = id;
            MaterialId = materialId;
            DocumentId = documentId;
            TotalPrice = toatalPrice;
            AmountOf = amountOf;
            Price = price;
            Description = descripion;
            SubmitDate = submitDate;
        }
        #endregion

        #region Properties
        public uint Price { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public int TotalPrice { get; set; }
        public double AmountOf { get; set; }
        #endregion
    }
}
