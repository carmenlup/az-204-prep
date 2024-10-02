// See https://aka.ms/new-console-template for more information
using CosmosDb;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
var host = builder.Build();

//add code here
var endpoint = builder.Configuration["CosmosDbAccount:EndpointUri"];
var accessKey = builder.Configuration["CosmosDbAccount:AccessKey"];
var dbId = "app-db";
var containerId = "Orders";

// create the DB
var accountClient = new CosmosClient(endpoint, accessKey);
var dbTask = Task.Run(async() =>await accountClient.CreateDatabaseIfNotExistsAsync(dbId)).GetAwaiter();
Database database = dbTask.GetResult().Database;
Console.WriteLine($"Db was created");

//create the container
var containerProp = new ContainerProperties(containerId, "/category");
var containerTask = Task.Run(async () => await database.CreateContainerIfNotExistsAsync(containerProp)).GetAwaiter();
Container container = containerTask.GetResult().Container;
Console.WriteLine($"container was created");
var entities = new List<Order>()
{
    new (
        Guid.NewGuid(),
       "O1",
       "Laptop",
       100
    ),
    new(
         Guid.NewGuid(),
        "O2",
        "Mobile",
        200
    ),
    new (
          Guid.NewGuid(),
        "O3",
         "Desktop",
         75
    ),
    new (
          Guid.NewGuid(),
        "O4",
         "Laptop",
         25
    )
};



await AddItem(entities, container);
async Task AddItem(List<Order> newItems, Container container)
{
    ItemResponse<Order> response;
    
    foreach (var item in newItems)
    {
        response = await container.CreateItemAsync<Order>(item, new PartitionKey(item.category));
        
        Console.WriteLine($"Order with OrderId = {item.orderId} was added");
        Console.WriteLine($"RU (request unit): {response.RequestCharge}");
    }
}

await host.RunAsync();
