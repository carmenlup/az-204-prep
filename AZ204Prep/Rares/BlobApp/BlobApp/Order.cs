using Azure.Data.Tables;
using Azure;

namespace BlobApp
{
    public class Order : ITableEntity
    {
        public string orderId { get; set; }
        public string category { get; set; }

        public int quantity { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}