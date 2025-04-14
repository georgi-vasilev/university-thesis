namespace Application.Order.Commands.Complete
{
    using Domain.Order;

    public record OrderCompleteOutputModel
    {
        public OrderCompleteOutputModel(Guid orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }

        public Guid OrderId { get; init; }
        public OrderStatus Status { get; init; }
    }
}
