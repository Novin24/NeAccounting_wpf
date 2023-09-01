using Domain.Common;

namespace Domain.NovinEntity.Customers
{
    public class Customer : LocalEntity<Guid>
    {
        #region navigation

        #endregion

        #region ctor
        internal Customer() { }

        internal Customer(string name,
            string mobile,
            uint etebar,
            string nationalCode,
            string address,
            bool isBuyer,
            bool isSeller)
        {
            Name = name;
            Mobile = mobile;
            Etebar = etebar;
            NationalCode = nationalCode;
            Address = address;
            Buyer = isBuyer;
            Seller = isSeller;
        }
        #endregion

        #region properties
        public string Name { get; set; }
        public string Mobile { get; set; }
        public uint Etebar { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public bool Buyer { get; set; }
        public bool Seller { get; set; }
        #endregion
    }
}
