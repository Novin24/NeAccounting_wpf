using Domain.Common;

namespace Domain.NovinEntity.Services
{
    public class Service : LocalEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Active { get; set; }


        public Service(string name, int price)
        {
            Name = name;
            Price = price;
            Active = true;
        }
    }
}