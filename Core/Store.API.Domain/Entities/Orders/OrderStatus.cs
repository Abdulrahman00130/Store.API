namespace Store.API.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        Pending = 0,
        PaymentSuccess,
        PaymentFailed
    }
}