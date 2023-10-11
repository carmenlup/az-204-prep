using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class QueueFunc2
    {
        // The function expects the message to be base-64 encoded, a format that will be safely read an consumed by other systems
        // The function will try to dequeue the message multiple times (5 times - MaxDequeueCount). If it can't, it creates a [queueName]-poison queue and will move the message there
        //
        [FunctionName("GetMessagesFromQueue2")]
        public void Run([QueueTrigger("queue-liviu-3", Connection = "funcConnectionString")]Order myQueueItem
                        , ILogger log
                        // This is another way of how to insert into a Table
                        , [Table("Orders", Connection = "funcConnectionString")] ICollector<Order> collector)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem.RowKey}, {myQueueItem.PartitionKey}, {myQueueItem.Quantity}");

            collector.Add(myQueueItem);
        }
    }
}
