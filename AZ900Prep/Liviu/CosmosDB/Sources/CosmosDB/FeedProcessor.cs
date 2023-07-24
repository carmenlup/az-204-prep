using CosmosDB.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDB
{
    internal class FeedProcessor
    {
        public static async Task StopProcessor(ChangeFeedProcessor changeFeedProcessor)
        {
            await changeFeedProcessor.StopAsync();

            Console.WriteLine("The Change Feed Processor has been stopped.");
        }

        public static async Task<ChangeFeedProcessor> StartProcessor()
        {
            var leasesContainer = DbUtilities.GetContainer(DbUtilities.leasesContainerName);
            var monitorContainer = DbUtilities.GetContainer(DbUtilities.customerContainerName);

            var feedProcessor = monitorContainer
                                .GetChangeFeedProcessorBuilder<Customer>(nameof(ManageChanges), ManageChanges)
                                .WithInstanceName("appHost")
                                .WithLeaseContainer(leasesContainer)
                                .Build();

            await feedProcessor.StartAsync();

            Console.WriteLine("The Change Feed Processor has been started - The Customers container is monitored.");

            return feedProcessor;

        }

        public static async Task ManageChanges(
                                    ChangeFeedProcessorContext context,
                                    IReadOnlyCollection<Customer> itemCollection,
                                    CancellationToken cancellationToken)
        {
            foreach (Customer item in itemCollection)
            {
                Console.WriteLine(item + " has been processed by our Delegate");
            }
        }
    }
}
