using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;

namespace Domain.NovinEntity.Documents
{
    public class Document : LocalEntity<Guid>
    {
        #region Navigation
        public Guid CustomerId { get; private set; }
        public IEnumerable<SellRemittance> SellRemittances { get; private set; }
        public IEnumerable<BuyRemittance> BuyRemittances { get; private set; }
        #endregion

        #region Ctor
        internal Document()
        {
            SellRemittances = new List<SellRemittance>();
            BuyRemittances = new List<BuyRemittance>();
        }

        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            string? descripion,
            DateTime submitDate,
            bool isReceived)
        {
            CustomerId = customerId;
            Price = price;
            Type = type;
            Description = descripion;
            SubmitDate = submitDate;
            IsReceived = isReceived;
        }
        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            string? descripion,
            DateTime submitDate,
            bool isReceived,
            List<SellRemittance> sellRemittances) : this(customerId, price, type, descripion, submitDate, isReceived)
        {
            SellRemittances = sellRemittances;
        }

        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            string? descripion,
            DateTime submitDate,
            bool isReceived,
            List<BuyRemittance> buyRemittances) : this(customerId, price, type, descripion, submitDate, isReceived)
        {
            BuyRemittances = buyRemittances;
        }
        #endregion

        #region Properties
        public long Price { get; set; }
        public string? Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public DocumntType Type { get; set; }
        /// <summary>
        /// ما دریافت کردیم
        /// </summary>
        public bool IsReceived { get; set; }
        public long Serial { get; set; }
        #endregion
    }
}
