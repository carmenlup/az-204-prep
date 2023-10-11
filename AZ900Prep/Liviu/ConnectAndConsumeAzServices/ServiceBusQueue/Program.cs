using Azure.Messaging.ServiceBus;
using ServiceBusQueue;

var order = new Order(1, "laptop", 11);
await SendSingleMessage(order);

var orders = new List<Order>
{
    new Order(2, "cars", 3),
    new Order(3, "books", 4),
    new Order(4, "laptops", 23),
};
await SendMultipleMessages(orders);

//await PeekMessages();

await ReceiveMessages();

async Task SendSingleMessage(Order order)
{
    ServiceBusSender sender = CreateQueueSender();

    var message = new ServiceBusMessage(order.ToJson())
    {
        ContentType = "application/json"
    };

    await sender.SendMessageAsync(message);

    Console.WriteLine("Sent single message");
}

async Task SendMultipleMessages(IEnumerable<Order> orders)
{
    ServiceBusSender sender = CreateQueueSender();
    
    var messageBatch = await sender.CreateMessageBatchAsync();
    foreach (var order in orders)
    {
        var message = new ServiceBusMessage(order.ToJson())
        {
            ContentType = "application/json"
        };
        messageBatch.TryAddMessage(message);
    }

    await sender.SendMessagesAsync(messageBatch);

    Console.WriteLine($"Sent {orders.Count()} messages");
}

async Task PeekMessages()
{
    ServiceBusReceiver receiver = CreatePeekReceiver();

    // this will continue polling messages from the queue - it doesn't end after the last message
    IAsyncEnumerable<ServiceBusReceivedMessage> messages = receiver.ReceiveMessagesAsync();

    Console.WriteLine($"Peek messages:");
    await foreach (var message in messages)
    {
        Console.WriteLine(message.Body.ToObjectFromJson<Order>().ToString());
    }

}

async Task ReceiveMessages()
{
    ServiceBusReceiver receiver = CreateMessageReceiver();

    // this will continue polling messages from the queue - it doesn't end after the last message
    IAsyncEnumerable<ServiceBusReceivedMessage> messages = receiver.ReceiveMessagesAsync();

    Console.WriteLine($"Received messages:");
    await foreach (var message in messages)
    {
        Console.WriteLine(message.Body.ToObjectFromJson<Order>().ToString());
    }
}


// we need to go to our Queue and create a Shared Access Policy, which will give us the connection string that will allow us to access the queue
const string c_connectionString = "";
const string c_queueName = "service-bus-queue-1";
ServiceBusSender CreateQueueSender()
{
    ServiceBusClient client = new ServiceBusClient(c_connectionString);
    return client.CreateSender(c_queueName);
}

ServiceBusReceiver CreatePeekReceiver()
{
    ServiceBusClient client = new ServiceBusClient(c_connectionString);
    var options = new ServiceBusReceiverOptions()
    {
        ReceiveMode = ServiceBusReceiveMode.PeekLock
    };

    return client.CreateReceiver(c_queueName, options);
}

ServiceBusReceiver CreateMessageReceiver()
{
    ServiceBusClient client = new ServiceBusClient(c_connectionString);
    var options = new ServiceBusReceiverOptions()
    {
        ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
    };

    return client.CreateReceiver(c_queueName, options);
}
