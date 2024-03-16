using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainShared.Enums;
using System.Threading.Tasks;
using DomainShared.Enums.StatusCheque;

namespace Domain.NovinEntity.Cheque
{
    public class Chequ
    {
        #region Navigation

        #endregion

        #region Property
        public string Name { get; set; }
        public ChequeStatus Status { get; set; }
        public int Price { get; set; }
        public System.DateTime SubmitDate { get; set; }
        public System.DateTime TransferdDate { get; set; }
        public System.DateTime Recived_Date { get; set; }
        public System.DateTime Due_Date { get; set; }
        public string Cheque_Number { get; set; }
        public string Serial { get; set; }
        public string Accunt_Number { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Branch { get; set; }
        public string Cheque_Owner { get; set; }
        public int Payer { get; set; }
        public int Reciver { get; set; }
        public string Description { get; set; }
        public string IsConvertedToCash { get; set; }
        public bool Archive { get; set; }
        public bool Deleted { get; set; } = false;
        public bool DeletedInNewYear { get; set; }
        #endregion
        #region Constructor
        public Chequ(string name,
            string bank_Name,
            string bank_Branch,
            string cheque_Number,
            int price, 
            string accunt_Number,
            string cheque_Owner,
            ChequeStatus status,
            string description)
        {
            Name = name;
            Bank_Name = bank_Name;
            Bank_Branch = bank_Branch;
            Cheque_Number = cheque_Number;
            Price = price;
            Accunt_Number = accunt_Number;
            Cheque_Owner = cheque_Owner;
            Status = status;
            Description = description;

        }
        #endregion

    }
}
