namespace QueueFunction
{
    public class Order
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }

        public int Quantity { get; set; }
    }
}
