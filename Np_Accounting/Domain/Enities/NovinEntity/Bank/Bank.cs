using Domain.Common;

namespace Domain.Enities.NovinEntity.Bank
{
    public class Banks : LocalEntity
    {
        public int Bank_ID { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Branch { get; set; }
        public string Account_num { get; set; }

    }
}
