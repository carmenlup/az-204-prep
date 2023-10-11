using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class QueueFunc1
    {
        // The function expects the message to be base-64 encoded, a format that will be safely read an consumed by other systems
        // The function will try to dequeue the message multiple times (5 times - MaxDequeueCount). If it can't, it creates a [queueName]-poison queue and will move the message there
        //
        [FunctionName("GetMessagesFromQueue")]
        // This is how you can insert the output of the function into a Table
        [return: Table("Orders", Connection = "funcConnectionString")]
        public Order Run([QueueTrigger("queue-liviu-3", Connection = "funcConnectionString")]Order myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem.RowKey}, {myQueueItem.PartitionKey}, {myQueueItem.Quantity}");

            return myQueueItem;
        }
    }
}
