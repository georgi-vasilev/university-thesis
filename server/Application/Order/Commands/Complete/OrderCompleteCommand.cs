namespace Application.Order.Commands.Complete
{
    using ErrorOr;
    using MediatR;

    public record OrderCompleteCommand : IRequest<ErrorOr<OrderCompleteOutputModel>>
    {
        public OrderCompleteCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
