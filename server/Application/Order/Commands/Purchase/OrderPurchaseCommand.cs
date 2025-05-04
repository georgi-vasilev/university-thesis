namespace Application.Order.Commands.Purchase
{
    using Domain.Order;
    using ErrorOr;
    using MediatR;

    public record OrderPurchaseCommand(
        Guid EventId,
        int Quantity,
        TicketType TicketType,
        string PaymentIntentId) : IRequest<ErrorOr<Success>>;
}
