namespace ViewLayer.Models
{
    public class Tariff
    {
        public Tariff(int price, string name, int connectionSpeed)
        {
            Price = price;
            Name = name;
            ConnectionSpeed = connectionSpeed;
        }
        public Tariff() { }
        public int Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public int ConnectionSpeed { get; set; }
    }
}
