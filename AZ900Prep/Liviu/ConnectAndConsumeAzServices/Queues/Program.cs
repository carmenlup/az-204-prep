using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

const string c_storageAccountConnString = "";
const string c_queueName = "queue-liviu-3";

int length = GetQueueLength();
const bool c_useBase64Encoding = true;
if (length == 0)
{
    SendMessages(3, c_useBase64Encoding);
}

PeekMessages(c_useBase64Encoding);

ReceiveMessages(2, c_useBase64Encoding);



void SendMessages(int count, bool isBase64 = false)
{

    Console.WriteLine($"Sending {count} messages to the queue!");
    var client = new QueueClient(c_storageAccountConnString, c_queueName);

    for (int i = 0; i < count; i++)
    {
        string message = $"Message {i + 1}";
        Console.WriteLine($"Message: {message}, has been sent!");

        var bytes = System.Text.Encoding.UTF8.GetBytes(message);
        var base64Message = System.Convert.ToBase64String(bytes);

        message = isBase64 ? base64Message : message;


        client.SendMessage(message);

    }
}

// The messages are not removed from the queue
void PeekMessages(bool isBase64 = false)
{
    var client = new QueueClient(c_storageAccountConnString, c_queueName);
    const int c_maxMessages = 10;

    PeekedMessage[] messages = client.PeekMessages(c_maxMessages);
    Console.WriteLine($"Peek operation: The top {c_maxMessages} messages in the queue are: ");
    foreach (PeekedMessage message in messages)
    {
        string queueMessage = message.Body.ToString();

        if (isBase64)
        {
            var base64String = System.Convert.FromBase64String(queueMessage);
            queueMessage = System.Text.Encoding.UTF8.GetString(base64String);
        }
        
        Console.WriteLine(queueMessage);
    }
}

int GetQueueLength()
{
    var queueClient = new QueueClient(c_storageAccountConnString, c_queueName);
    var queueLength = 0;

    if (queueClient.Exists())
    {
        QueueProperties properties = queueClient.GetProperties();
        queueLength = properties.ApproximateMessagesCount;
    }
    else
    {
        queueClient.Create();
    }

    Console.WriteLine($"The length of queue {c_queueName} is {queueLength}");

    return queueLength;
}


// The received messages are removed from the queue for a period defined by the visibilityTimeout parameter (30 sec by default). They are added back after that
// Each time the message is 'Received' the Dequeue Count property is incremented by 1
// As a normal flow - Once the app 'Received' the message and successfully processed it, it should delete it 
void ReceiveMessages(int count, bool isBase64 = false)
{
    var client = new QueueClient(c_storageAccountConnString, c_queueName);

    QueueMessage[] messages = client.ReceiveMessages(count);
    foreach (QueueMessage message in messages)
    {
        ProcessMessage(message, isBase64);
        DeleteMessage(message);
    }
}


void ProcessMessage(QueueMessage message, bool isBase64)
{
    string queueMessage = message.Body.ToString();

    if (isBase64)
    {
        var base64String = System.Convert.FromBase64String(queueMessage);
        queueMessage = System.Text.Encoding.UTF8.GetString(base64String);
    }

    Console.WriteLine("Processing message: ");
    Console.WriteLine(queueMessage);
}

void DeleteMessage(QueueMessage queueMessage)
{
    var client = new QueueClient(c_storageAccountConnString, c_queueName);
    Console.WriteLine($"Deleting message: MessageId = {queueMessage.MessageId}, PopReceipt = {queueMessage.PopReceipt}");
    client.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
}