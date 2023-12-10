using CosmosDB.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using System.Net;

namespace CosmosDB
{
    internal class DbUtilities
    {
        internal static readonly string connectionUri = "";
        internal static readonly string connectionKey = "";
        internal static readonly string dbName = "Database2";
        internal static readonly string ordersContainerName = "Orders";
        internal static readonly string customerContainerName = "Customers";
        internal static readonly string leasesContainerName = "leases";

        // The partition key have to be prefixed with slash /
        internal static readonly string orderPatitionKey = "/Category";
        internal static readonly string customerPatitionKey = "/City";
        internal static readonly string leasesPatitionKey = "/id";

        internal async static Task InitializeDB()
        {
            Console.WriteLine("Start creating DB");

            await DeleteDBIfExits();

            await CreateDB();

            await CreateContainer(ordersContainerName, orderPatitionKey);
            await CreateTrigger(ordersContainerName);
            await CreateStoredProcedure(ordersContainerName);

            await CreateContainer(customerContainerName, customerPatitionKey);
            await CreateContainer(leasesContainerName, leasesPatitionKey);
        }

        internal static async Task CreateContainer(string containerName, string partitionKey)
        {
            // create containers (tables as we know in SQL)
            var cosmosClient = new CosmosClient(connectionUri, connectionKey);
            var db = cosmosClient.GetDatabase(dbName);

            await db.CreateContainerAsync(containerName, partitionKey);

            Console.WriteLine($"Container {containerName} has been created.");
        }

        internal async static Task DeleteDBIfExits()
        {
            var cosmosClient = new CosmosClient(connectionUri, connectionKey);
            var db = cosmosClient.GetDatabase(dbName);
            try
            {
                var response = await db.ReadAsync();

                // if no exception is thrown it means the DB exists
                await db.DeleteAsync();
                Console.WriteLine($"DB {dbName} has been deleted.");
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    // Does not exist
                }
            }
        }

        internal async static Task CreateDB()
        {
            await CosmosClient.CreateDatabaseAsync(dbName);

            Console.WriteLine($"DB {dbName} has been created.");
        }

        internal static Container GetContainer(string containerName)
        {
            var cosmosClient = new CosmosClient(connectionUri, connectionKey);
            var db = cosmosClient.GetDatabase(dbName);
            var container = db.GetContainer(containerName);

            return container;
        }

        internal static async Task CreateTrigger(string containerName)
        {
            var container = GetContainer(containerName);
            await container.Scripts.CreateTriggerAsync(
                new TriggerProperties()
                {
                    Id = "ValidateTrigger",
                    TriggerOperation = TriggerOperation.Create,
                    TriggerType = TriggerType.Pre,
                    Body = File.ReadAllText("Triggers\\ValidateTrigger.js")
                });

            Console.WriteLine($"Trigger ValidateTrigger for container {containerName} has been created.");
        }

        internal static async Task CreateStoredProcedure(string containerName)
        {
            var container = GetContainer(containerName);
            await container.Scripts.CreateStoredProcedureAsync(new StoredProcedureProperties
            {
                Id = "AddItems",
                Body = File.ReadAllText("StoredProcedures\\AddItems.js")
            });

            Console.WriteLine($"Stored Procedure AddItems for container {containerName} has been created.");
        }

        internal static CosmosClient CosmosClient => new CosmosClient(connectionUri, connectionKey);
    }
}
