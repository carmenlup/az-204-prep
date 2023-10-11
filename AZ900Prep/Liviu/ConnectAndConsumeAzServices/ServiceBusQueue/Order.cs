using Newtonsoft.Json;

namespace ServiceBusQueue
{
    public class Order
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public int Quantity { get; set; }

        public Order(int id, string category, int quantity)
        {
            Id = id;
            Category = category;
            Quantity = quantity;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Order CreateFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Order>(json);
        }

        public override string ToString()
        {
            return $"Order: {Id}, {Category}, {Quantity}";
        }
    }
}
