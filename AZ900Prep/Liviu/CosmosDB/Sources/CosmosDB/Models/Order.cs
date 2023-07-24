using Newtonsoft.Json;

namespace CosmosDB.Models
{
    public class Order
    {
        [JsonProperty("id")]
        public string OrderId { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"Order {OrderId}, Category = {Category}, Quantity = {Quantity}";
        }
    }
}
