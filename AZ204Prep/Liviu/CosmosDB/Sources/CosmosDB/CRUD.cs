using CosmosDB.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDB
{
    internal class CRUD
    {
        internal static async Task AddOrders()
        {
            var orderContainer = DbUtilities.GetContainer(DbUtilities.ordersContainerName);

            var newOrder = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Category = "Cars",
                Quantity = 2
            };
            ItemResponse<Order> itemResponse = await orderContainer.CreateItemAsync<Order>(newOrder);
            
            Console.WriteLine(itemResponse.Resource.ToString() + " has been created");

            newOrder = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Category = "laptop",
                Quantity = 3
            };
            await orderContainer.CreateItemAsync<Order>(newOrder);
            Console.WriteLine($"{newOrder} has been created");

            newOrder = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Category = "laptop",
                Quantity = 4
            };
            await orderContainer.CreateItemAsync<Order>(newOrder);
            Console.WriteLine($"{newOrder} has been created");
        }

        internal static async Task AddOrdersWithStoredProcedure()
        {
            var orderContainer = DbUtilities.GetContainer(DbUtilities.ordersContainerName);
            string partitionKeyForNewOrders = "OrderSP";

            List<Order> orders = new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    Category = partitionKeyForNewOrders,
                    Quantity = 2
                },
                new Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    Category = partitionKeyForNewOrders,
                    Quantity = 1
                }
            };

            await orderContainer.Scripts.ExecuteStoredProcedureAsync<Order>("AddItems", new PartitionKey(partitionKeyForNewOrders), new[] { orders });
            
            Console.WriteLine($"{orders.Count} orders have been created with the addItems SP.");
        }

        internal static async Task AddOrderWithTrigger()
        {
            var container = DbUtilities.GetContainer(DbUtilities.ordersContainerName);

            string orderId = Guid.NewGuid().ToString();
            dynamic newOrder = new
            {
                id = orderId,
                OrderId = orderId,
                Category = "CatWithDefaultQuantity"
            };

            await container.CreateItemAsync(newOrder, null, new ItemRequestOptions() { PreTriggers = new[] { "ValidateTrigger" } });

            Console.WriteLine("New order has been created - activate Trigger.");
        }

        internal static async Task AddCustomers()
        {
            var container = DbUtilities.GetContainer(DbUtilities.customerContainerName);

            var orders = new List<Order>
            {
                new Order { Category = "Laptop", OrderId = Guid.NewGuid().ToString(), Quantity = 3},
                new Order { Category = "Mobile", OrderId = Guid.NewGuid().ToString(), Quantity = 4}
            };
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                City = "Bucharest",
                CustomerName = "Liviu",
                Orders = orders
            };

            await container.CreateItemAsync<Customer>(customer);
            Console.WriteLine($"{customer} has been created.");

            orders = new List<Order>
            {
                new Order { Category = "Cars", OrderId = Guid.NewGuid().ToString(), Quantity = 3},
                new Order { Category = "Mobile", OrderId = Guid.NewGuid().ToString(), Quantity = 4}
            };
            customer = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                City = "Cluj",
                CustomerName = "John Doe",
                Orders = orders
            };

            await container.CreateItemAsync<Customer>(customer);
            Console.WriteLine($"{customer} has been created.");
        }

        internal static async Task ReadCustomer(string customerName)
        {
            var container = DbUtilities.GetContainer(DbUtilities.customerContainerName);

            var query = new QueryDefinition("select * from Customers c where c.CustomerName = @name")
                                .WithParameter("@name", customerName);

            FeedIterator<Customer> feedIteretor = container.GetItemQueryIterator<Customer>(query);
            while (feedIteretor.HasMoreResults)
            {
                var feedResponse = await feedIteretor.ReadNextAsync();
                if (feedResponse.Count > 0)
                {
                    foreach (var customer in feedResponse)
                    {
                        Console.WriteLine($"Item found: {customer}");
                    }
                }
                else
                {
                    Console.WriteLine($"Customer with name {customerName} was not found.");
                }                
            }
        }

        internal static async Task UpdateCustomer(string customerName)
        {
            var container = DbUtilities.GetContainer(DbUtilities.customerContainerName);

            var query = new QueryDefinition("select * from Customers c where c.CustomerName = @name")
                                .WithParameter("@name", customerName);

            FeedIterator<Customer> feedIteretor = container.GetItemQueryIterator<Customer>(query);
            while (feedIteretor.HasMoreResults)
            {
                var feedResponse = await feedIteretor.ReadNextAsync();
                foreach (var customer in feedResponse)
                {
                    customer.Orders = new List<Order>
                                             {
                                                 new Order{ Category = "NewOrderCategory", OrderId = Guid.NewGuid().ToString(), Quantity = 4 }
                                             };
                    await container.ReplaceItemAsync<Customer>(customer, customer.CustomerId);
                    Console.WriteLine($"{customer} has been updated!");
                }
            }
        }

        internal static async Task DeleteCustomer(string customerName)
        {
            var container = DbUtilities.GetContainer(DbUtilities.customerContainerName);

            var query = new QueryDefinition("select * from Customers c where c.CustomerName = @name")
                                .WithParameter("@name", customerName);

            FeedIterator<Customer> feedIteretor = container.GetItemQueryIterator<Customer>(query);
            while (feedIteretor.HasMoreResults)
            {
                var feedResponse = await feedIteretor.ReadNextAsync();

                foreach (var customer in feedResponse)
                {
                    await container.DeleteItemAsync<Customer>(customer.CustomerId, new PartitionKey(customer.City));
                    Console.WriteLine($"{customer} has been deleted!");
                }
            }
        }
    }
}
