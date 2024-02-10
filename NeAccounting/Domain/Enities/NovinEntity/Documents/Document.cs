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
            uint price,
            string descripion,
            DateTime submitDate,
            bool receivedOrPaid)
        {
            CustomerId = customerId;
            Price = price;
            Description = descripion;
            SubmitDate = submitDate;
            ReceivedOrPaid = receivedOrPaid;
        }
        public Document(
            Guid customerId,
            uint price,
            string descripion,
            DateTime submitDate,
            bool receivedOrPaid,
            List<SellRemittance> sellRemittances) : this(customerId, price, descripion, submitDate, receivedOrPaid)
        {
            SellRemittances = sellRemittances;
        }
        
        public Document(
            Guid customerId,
            uint price,
            string descripion,
            DateTime submitDate,
            bool receivedOrPaid,
            List<BuyRemittance> buyRemittances) : this(customerId, price, descripion, submitDate, receivedOrPaid)
        {
            BuyRemittances = buyRemittances;
        }
        #endregion

        #region Properties
        public uint Price { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public DocumntType Type { get; set; }
        public bool ReceivedOrPaid { get; set; }
        public long Serial { get; set; }
        #endregion
    }
}
