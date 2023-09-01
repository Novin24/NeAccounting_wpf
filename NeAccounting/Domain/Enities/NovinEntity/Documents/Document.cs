using Domain.Common;

namespace Domain.NovinEntity.Documents
{
    public class Document : LocalEntity<Guid>
    {
        #region Navigation
        public Guid CustomerId { get; private set; }
        #endregion

        #region Ctor
        internal Document() { }

        internal Document(Guid id,
            Guid customerId,
            int price,
            string descripion,
            DateTime submitDate,
            bool receivedOrPaid
            )
        {
            Id = id;
            CustomerId = customerId;
            Price = price;
            Description = descripion;
            SubmitDate = submitDate;
            ReceivedOrPaid = receivedOrPaid;
        }
        #endregion

        #region Properties
        public int Price { get; private set; }
        public string Description { get; private set; }
        public DateTime SubmitDate { get; private set; }
        public bool ReceivedOrPaid { get; private set; }
        public long Serial { get; private set; }
        #endregion
    }
}
