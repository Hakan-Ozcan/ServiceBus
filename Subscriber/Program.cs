using Azure.Messaging.ServiceBus;
using Contracts;

var connectionString = "Endpoint=sb://YOUR_SERVICE_BUS.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YOUR_SHAREDACCESSKEY";
var queueName = "order.status.queue";

var clientOptions = new ServiceBusClientOptions
{
    RetryOptions = new ServiceBusRetryOptions()
    {
        MaxRetries = 10,
        MaxDelay = TimeSpan.FromMinutes(1)
    }
};

await using var serviceBusClient = new ServiceBusClient(connectionString, clientOptions);


var options = new ServiceBusSessionProcessorOptions()
{
    SessionIds = { "my-x-sessions", "my-y-sessions" },
    AutoCompleteMessages = false
};
await using ServiceBusSessionProcessor sessionProcessor = serviceBusClient.CreateSessionProcessor(queueName, options);

sessionProcessor.ProcessMessageAsync += ProcessMessages;
sessionProcessor.ProcessErrorAsync += ProcessErrors;

Task ProcessMessages(ProcessSessionMessageEventArgs args)
{

    var orderStatusChangedEvent = args.Message.Body.ToObjectFromJson<OrderStatusChangedEvent>();

    Console.WriteLine($"Session ID: {args.SessionId}\nOrder ID: {orderStatusChangedEvent?.OrderId}\nStatus: {orderStatusChangedEvent?.Status}\nChanged Date: {orderStatusChangedEvent?.ChangeDate}");
    Console.WriteLine("------------------------------");

    var appProperties = args.Message.ApplicationProperties;

    if (appProperties != null && appProperties.TryGetValue("IsLastItem", out var isLastItem))
    {

        if ((bool)isLastItem)
        {
            Console.WriteLine("Session closed");
            args.ReleaseSession();

        }
    }

    return Task.CompletedTask;
}

Task ProcessErrors(ProcessErrorEventArgs args)
{

    Console.WriteLine("There is an error!");

    return Task.CompletedTask;
}

await sessionProcessor.StartProcessingAsync();
Console.ReadKey();