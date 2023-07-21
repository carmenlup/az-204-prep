using Newtonsoft.Json;

namespace CosmosDB.Models
{
    public class Customer
    {
        [JsonProperty("id")]
        public string CustomerId { get; set; }
        public string City { get; set; }
        public string CustomerName { get; set; }
        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Customer {CustomerName}, from {City}, with {Orders.Count} orders.";
        }
    }
}
