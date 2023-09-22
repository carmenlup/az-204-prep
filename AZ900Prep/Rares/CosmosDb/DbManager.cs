using Microsoft.Azure.Cosmos;

namespace CosmosDb;

public class DbManager
{
    string dbName = "appdb";
    string cosmosEndpointUri = "https://az-204-oosmos-db.documents.azure.com:443/";
    string cosmosKey = "rxK4pJ2CM5tGFtli9anF61OsdtYHXd1rDC8R7dRncYw7ck5CQ4QRSTqyzFHKoxrss5UKWelbqUvMACDbRXG7zA==";
    private readonly CosmosClient _cosmosClient;
    string containerName = "Orders";

    public DbManager()
    {
        _cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosKey);
    }

    public async Task CreateDb()
    {
        await _cosmosClient.CreateDatabaseAsync(dbName);

        Console.WriteLine($"Db {dbName} created");
    }

    public async Task CreateContainer(string containerName, string partitionKey)
    {
        var db = _cosmosClient.GetDatabase(dbName);

        await db.CreateContainerIfNotExistsAsync(containerName, partitionKey);
        Console.WriteLine($"Container {containerName} created");
    }

    public async Task AddItem(string orderId, string category, int quantity)
    {
        var db = _cosmosClient.GetDatabase(dbName);
        var container = db.GetContainer(containerName);

        var order = new Order()
        {
            id = Guid.NewGuid().ToString(),
            orderId = orderId,
            category = category,
            quantity = quantity
        };

        var response = await container.CreateItemAsync<Order>(order, new PartitionKey(category));

        Console.WriteLine($"Added item with orderId {orderId}");
        Console.WriteLine($"Request units: {response.RequestCharge}");
    }

    public async Task ReadItem()
    {
        var db = _cosmosClient.GetDatabase(dbName);
        var container = db.GetContainer(containerName);

        string sqlQuery = "SELECT o.orderId, o.category, o.quantity from Orders o";
        var query = new QueryDefinition(sqlQuery);

        var feed = container.GetItemQueryIterator<Order>(query);
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var order in response)
            {
                Console.WriteLine($"orderId : {order.id}");
                Console.WriteLine($"quanitty: {order.quantity}");
            }
        }
    }

    public async Task ReplaceItem()
    {
        var db = _cosmosClient.GetDatabase(dbName);
        var container = db.GetContainer(containerName);

        var orderId = "O1";
        string sqlQuery = $"SELECT o.id, o.category, o.quantity from Orders o WHERE o.orderId ='{orderId}'";

        var id = "";
        var category = "";

        var query = new QueryDefinition(sqlQuery);

        var feed = container.GetItemQueryIterator<Order>(query);
        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync();
            foreach (var order in feedResponse)
            {
                id = order.id;
                category = order.category;
            }
        }

        var response = await container.ReadItemAsync<Order>(id, new PartitionKey(category));
        var item = response.Resource;
        item.quantity = 123123;

        await container.ReplaceItemAsync<Order>(item, id, new PartitionKey(category));

        Console.WriteLine("item is updated");
    }

    public async Task DeleteItem()
    {
        var db = _cosmosClient.GetDatabase(dbName);
        var container = db.GetContainer(containerName);

        var orderId = "O1";
        string sqlQuery = $"SELECT o.id, o.category, o.quantity from Orders o WHERE o.orderId ='{orderId}'";

        var id = "";
        var category = "";

        var query = new QueryDefinition(sqlQuery);

        var feed = container.GetItemQueryIterator<Order>(query);
        while (feed.HasMoreResults)
        {
            var feedResponse = await feed.ReadNextAsync();
            foreach (var order in feedResponse)
            {
                id = order.id;
                category = order.category;
            }
        }

        var response = await container.DeleteItemAsync<Order>(id, new PartitionKey(category));

        Console.WriteLine("item is deleted");
    }
}
