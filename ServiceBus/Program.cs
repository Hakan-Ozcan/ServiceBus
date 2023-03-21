using Azure.Messaging.ServiceBus;
using Contracts;

var connectionString = "Endpoint=sb://YOUR_SERVICE_BUS.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YOUR_SHAREDACCESSKEY";
var queueName = "order.status.queue";

await using var serviceBusClient = new ServiceBusClient(connectionString);
ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);

var orderId = Guid.NewGuid().ToString();

// ------------------- Order status: In Preparation -------------------
var orderStatusChangedToInPreparationEvent = new OrderStatusChangedEvent(
    orderId: orderId,
    status: "InPreparation",
    changeDate: DateTime.UtcNow);

var orderStatusChangedToInPreparationMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(orderStatusChangedToInPreparationEvent))
{
    SessionId = orderId
};
await serviceBusSender.SendMessageAsync(orderStatusChangedToInPreparationMessage);


// ------------------- Order status: Shipped -------------------
var orderStatusChangedToShippedEvent = new OrderStatusChangedEvent(
    orderId: orderId,
    status: "Shipped",
    changeDate: DateTime.UtcNow);

var orderStatusChangedToShippedMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(orderStatusChangedToShippedEvent))
{
    SessionId = orderId
};
await serviceBusSender.SendMessageAsync(orderStatusChangedToShippedMessage);


// ------------------- Order status: Delivery Attempt Failed -------------------
var orderStatusChangedToDeliveryAttemptFailedEvent = new OrderStatusChangedEvent(
    orderId: orderId,
    status: "DeliveryAttemptFailed",
    changeDate: DateTime.UtcNow);

var orderStatusChangedToDeliveryAttemptFailedMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(orderStatusChangedToDeliveryAttemptFailedEvent))
{
    SessionId = orderId
};
await serviceBusSender.SendMessageAsync(orderStatusChangedToDeliveryAttemptFailedMessage);


// ------------------- Order status: Delivered to Pickup Point -------------------
var orderStatusChangedToDeliveredToPickupPointEvent = new OrderStatusChangedEvent(
    orderId: orderId,
    status: "DeliveredToPickupPoint",
    changeDate: DateTime.UtcNow);

var orderStatusChangedToDeliveredToPickupPointMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(orderStatusChangedToDeliveredToPickupPointEvent))
{
    SessionId = orderId
};
await serviceBusSender.SendMessageAsync(orderStatusChangedToDeliveredToPickupPointMessage);


// ------------------- Order status: Completed -------------------
var orderStatusChangedToCompletedEvent = new OrderStatusChangedEvent(
    orderId: orderId,
    status: "Completed",
    changeDate: DateTime.UtcNow);

var orderStatusChangedToCompletedMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(orderStatusChangedToCompletedEvent))
{
    SessionId = orderId,
};
orderStatusChangedToCompletedMessage.ApplicationProperties.Add("IsLastItem", true);

await serviceBusSender.SendMessageAsync(orderStatusChangedToCompletedMessage);
