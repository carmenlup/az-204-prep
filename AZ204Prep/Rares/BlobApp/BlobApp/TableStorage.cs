using Azure;
using Azure.Data.Tables;

namespace BlobApp
{
    public class TableStorage
    {
        private const string connectionString = "";

        // for cosmos table api:
        //private const string connectionString = "";

        private const string tableName = "Orders";
        private TableClient _client;

        public TableStorage()
        {
            _client = new TableClient(connectionString, tableName);
        }

        public void AddEntities()
        {
            AddEntity("O1", "Mobile", 10);
            AddEntity("O2", "Pc", 134);
            AddEntity("O3", "Desk", 1032);
            AddEntity("O4", "Phone", 213);
        }

        public void GetEntityAndDeleteThem()
        {
            var results = _client.Query<TableEntity>();

            foreach (var entity in results)
            {
                Console.WriteLine($"Order id {entity.RowKey}, quantity: {entity.GetInt32("quantity")}");

                DeleteEntity(entity.PartitionKey, entity.RowKey);
            }
        }

        public void QueryEntity(string category)
        {
            var results = _client.Query<TableEntity>(x => x.PartitionKey == category);

            foreach (var entity in results)
            {
                Console.WriteLine($"Order id {entity.RowKey}, quantity: {entity.GetInt32("quantity")}");

                DeleteEntity(entity.PartitionKey, entity.RowKey);
            }
        }

        private void AddEntity(string orderId, string category, int quantity)
        {
            var table = _client.CreateIfNotExists();
            var tableEntity = new TableEntity(category, orderId) { { "quantity", quantity } };

            _client.AddEntity(tableEntity);
            Console.WriteLine($"Added entity with order id {orderId}");
        }

        public void DeleteEntity(string category, string orderId)
        {
            _client.DeleteEntity(category, orderId);
            Console.WriteLine($"Entity deleted with orderId {orderId} and category {category}");
        }

        public void UpdateEntity(string category, string orderID, int quantity)
        {
            Order order = _client.GetEntity<Order>(category, orderID);
            order.quantity = quantity;

            _client.UpdateEntity(order, ifMatch: ETag.All, TableUpdateMode.Replace);

            Console.WriteLine("Entity updated");
        }
    }
}
