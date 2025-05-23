﻿using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Cheques;
using DomainShared.Enums;
using DomainShared.Errore;

namespace Domain.NovinEntity.Documents
{
    public class Document : LocalEntity<Guid>
    {
        #region Navigation
        public Guid CustomerId { get; set; }
        public Guid? DocumentId { get; private set; }
        public Document P_Document { get; set; }
        public List<Cheque> Cheques { get; set; }
        public List<Document> RelatedDocuments { get; private set; }
        public List<SellRemittance> SellRemittances { get; private set; }
        public List<BuyRemittance> BuyRemittances { get; private set; }
        #endregion

        #region Ctor
        internal Document()
        {

        }

        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool isReceived)
        {
            CustomerId = customerId;
            Price = price;
            Type = type;
            PayType = payType;
            SetDesc(descripion);
            SubmitDate = submitDate;
            IsReceived = isReceived;
            RelatedDocuments = [];
            SellRemittances = [];
            BuyRemittances = [];
            Cheques = [];
        }


        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool isReceived,
            byte commission) : this(customerId, price, type, payType, descripion, submitDate, isReceived)
        {
            Commission = commission;
        }
        #endregion

        #region Properties
        public long Price { get; set; }
        public string? Description { get; private set; }
        /// <summary>
        /// تاریخ دریافت سند
        /// </summary>
        public DateTime SubmitDate { get; set; }
        public PaymentType PayType { get; set; }
        public DocumntType Type { get; set; }
        /// <summary>
        /// ما دریافت کردیم
        /// </summary>
        public bool IsReceived { get; set; }
        public byte? Commission { get; set; }
        public long Serial { get; set; }
        #endregion

        #region Methods
        public Document SetDesc(string? desc)
        {
            if (!string.IsNullOrEmpty(desc) && desc.Length > 150)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "صدوپنجاه"));
            }
            Description = desc;
            return this;
        }

        public Document AddSellRemittance(List<SellRemittance> list)
        {
            SellRemittances.AddRange(list);
            return this;
        }

        public Document AddBuyRemittance(List<BuyRemittance> list)
        {
            BuyRemittances.AddRange(list);
            return this;
        }

        public Document RemoveSellRem(SellRemittance inv)
        {
            SellRemittances.Remove(inv);
            return this;
        }

        public Document RemoveBuyRem(BuyRemittance inv)
        {
            BuyRemittances.Remove(inv);
            return this;
        }

        public Document AddDocument(List<Document> list)
        {
            RelatedDocuments.AddRange(list);
            return this;
        }

        public Document AddCheque(Cheque cheque)
        {
            Cheques.Add(cheque);
            return this;
        }
        #endregion
    }
}
