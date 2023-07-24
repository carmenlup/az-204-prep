using CosmosDB;
using Microsoft.Azure.Cosmos;

const string exit = "9";

DisplayMenu();
var option = Console.ReadLine();
ChangeFeedProcessor changeFeedProcessor = null;

while (option != exit)
{
    switch (option)
    {
        case "1":
            {
                // Create DB - re-runnable
                WriteLine("----------------------[Initialize DB]----------------------");
                await DbUtilities.InitializeDB();
            }; break;

        case "2":
            {
                // Add orders
                WriteLine("----------------------[Add orders]----------------------");
                await CRUD.AddOrders();
                await CRUD.AddOrdersWithStoredProcedure();
                await CRUD.AddOrderWithTrigger();

            }; break;
        case "3":
            {
                // Add Customers
                WriteLine("----------------------[Add customers]----------------------");
                await CRUD.AddCustomers();
            }; break;
        case "4":
            {
                // Read
                WriteLine("----------------------[Read customer]----------------------");
                await CRUD.ReadCustomer("Liviu");
            }; break;
        case "5":
            {
                // Update
                WriteLine("----------------------[Update customer]----------------------");
                await CRUD.UpdateCustomer("Liviu");
            }; break;
        case "6":
            {
                // Delete
                WriteLine("----------------------[Delete customer]----------------------");
                await CRUD.DeleteCustomer("Liviu");
            }; break;
        case "7":
            {
                // Start
                WriteLine("----------------------[Start Change Feed Processor]----------------------");
                changeFeedProcessor = await FeedProcessor.StartProcessor();
            }; break;
        case "8":
            {
                // Stop
                WriteLine("----------------------[Stop Change Feed Processor]----------------------");
                if (changeFeedProcessor != null)
                {
                    await FeedProcessor.StopProcessor(changeFeedProcessor);
                }
            }; break;

        default:
            Console.WriteLine("Invalid option."); break;
    }

    DisplayMenu();
    option = Console.ReadLine();
}

WriteLine("Press any key to exit...");
Console.ReadLine();

static void WriteLine(string text)
{
    Console.WriteLine();
    Console.WriteLine(text);
}

static void DisplayMenu()
{
    Console.WriteLine("---------------Menu---------------");
    Console.WriteLine("1 - Initialize DB.");
    Console.WriteLine("2 - Insert orders.");
    Console.WriteLine("3 - Insert customers.");
    Console.WriteLine("4 - Read customer Liviu.");
    Console.WriteLine("5 - Update customer Liviu.");
    Console.WriteLine("6 - Delete customer Liviu.");
    Console.WriteLine("7 - Start Feed Processor - Monitoring Customers.");
    Console.WriteLine("8 - Stop Feed Processor.");
    Console.WriteLine("9 - Exit");
    Console.Write("Type your option = ");
}