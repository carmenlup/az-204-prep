using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using Azure;

namespace StorageAccountApp
{
    public class TableService
    {

        string _connectionString = "";
        string _tableName = "Orders";

        public async Task RunTableServiceCommandsAsync()
        {

            await CreateTableAsync();

            await AddOrdersAsync();

            ReadByCategory("laptop");

            DeleteOrder("laptop", "1");

            ReadByCategory("laptop");

            UpdateOrder("laptop", "4");

            ReadByCategory("laptop");

        }

        async Task AddOrdersAsync()
        {
            List<Order> orders = new()
    {
        new Order{ Category = "laptop", OrderId = 1, Quantity = 20},
        new Order{ Category = "mobile", OrderId = 2, Quantity = 32},
        new Order{ Category = "pc", OrderId = 3, Quantity = 11},
        new Order{ Category = "laptop", OrderId = 4, Quantity = 2},
    };

            var tableClient = GetTableClient(_tableName);
            foreach (var order in orders)
            {
                var newEntity = new TableEntity(order.Category, order.OrderId.ToString())
        {
            {"Quantity", order.Quantity}
        };

                await tableClient.AddEntityAsync(newEntity);

                Console.WriteLine($"Order {order.OrderId} has been inserted into table {_tableName}.");
            }

            Console.WriteLine();
        }

        void ReadByCategory(string categoryValue)
        {
            var client = GetTableClient(_tableName);
            var result = client.Query<TableEntity>(entity => entity.PartitionKey == categoryValue);
            Console.WriteLine($"Found {result.Count()} orders with category = {categoryValue}");
            foreach (var entity in result)
            {
                Console.WriteLine($"OrderId = {entity.RowKey}, Category = {entity.PartitionKey}, Quantity = {entity.GetInt32("Quantity")}");
            }

            Console.WriteLine();
        }

        void DeleteOrder(string partitionKey, string rowKey)
        {
            var client = GetTableClient(_tableName);
            client.DeleteEntity(partitionKey, rowKey);
            Console.WriteLine($"Order with category = {partitionKey} and id = {rowKey} has been deleted");
            Console.WriteLine();
        }

        void UpdateOrder(string partitionKey, string rowKey)
        {
            var client = GetTableClient(_tableName);
            Response<TableEntity> order = client.GetEntity<TableEntity>(partitionKey, rowKey);
            order.Value["Quantity"] = 123;

            client.UpdateEntity(order.Value, ETag.All, TableUpdateMode.Replace);

            Console.WriteLine("The order has been updated");
            Console.WriteLine();
        }

        async Task CreateTableAsync()
        {
            var result = await CreateIfNotExistsAsync(_tableName);
            if (TableExists(result)) // already exists
            {
                // re-create the table
                await DeleteTableAsync(_tableName);

                _tableName = _tableName + DateTime.Now.ToString("HHmmss"); // cannot re-create with the same name because the delete operation takes some time
                await CreateIfNotExistsAsync(_tableName);
            }

            Console.WriteLine($"Table {_tableName} has been created.");
            Console.WriteLine();
        }

        async Task<Response<TableItem>> CreateIfNotExistsAsync(string tableName)
        {
            var tableClient = GetTableClient(tableName);
            Response<TableItem> result = await tableClient.CreateIfNotExistsAsync();
            return result;
        }

        async Task DeleteTableAsync(string tableName)
        {
            var tableClient = GetTableClient(tableName);
            await tableClient.DeleteAsync();

            Console.WriteLine($"Table {tableName} has been deleted.");
            Console.WriteLine();
        }

        bool TableExists(Response<TableItem> tableCreateResponse)
        {
            return tableCreateResponse.GetRawResponse().Status == 409;
        }

        TableClient GetTableClient(string tableName) => new TableClient(_connectionString, tableName);
    }

}
