namespace Application.Order.Commands.Create
{
    using Domain.Order;

    public record CreateOrderOutputModel
    {
        public CreateOrderOutputModel(Guid orderId,
        Guid buyerId,
        OrderStatus status)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            Status = status;
        }

        public Guid OrderId { get; init; }
        public Guid BuyerId { get; init; }
        public OrderStatus Status { get; init; }
    }
}
