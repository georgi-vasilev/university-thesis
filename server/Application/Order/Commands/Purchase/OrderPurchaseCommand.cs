namespace Application.Order.Commands.Purchase
{
    using Domain.Common.ValueObject;
    using Domain.Order;
    using ErrorOr;
    using MediatR;

    public record OrderPurchaseCommand : IRequest<ErrorOr<Success>>
    {
        public Guid BuyerId { get; set; }
        public Guid EventId { get; set; }
        public Money PaymentAmount { get; set; } = default!;
        public TicketType TicketType { get; set; }
    }
}
