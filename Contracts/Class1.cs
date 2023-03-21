namespace Contracts;

public class OrderStatusChangedEvent
{
    public OrderStatusChangedEvent(string orderId, string status, DateTime changeDate)
    {
        OrderId = orderId;
        Status = status;
        ChangeDate = changeDate;
    }

    public string OrderId { get; set; }
    public string Status { get; set; }
    public DateTime ChangeDate { get; set; }
}